using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public abstract class DbContextBase : DbContext
    {
        private IDataFilter _dataFilter;
        private IAuditPropertySetter _auditPropertySetter;
        private IDomainEventsDispatcher _domainEventsDispatcher;

        /// <inheritdoc />
        protected DbContextBase()
        {
        }

        /// <inheritdoc />
        protected DbContextBase([NotNull] DbContextOptions options) : base(options)
        {
            Initialize();
        }

        private void Initialize()
        {
            _dataFilter = this.GetRequiredService<IDataFilter>();
            _auditPropertySetter = this.GetRequiredService<IAuditPropertySetter>();
            _domainEventsDispatcher = this.GetRequiredService<IDomainEventsDispatcher>();
        }

        protected virtual void UpdateConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is not IHasConcurrencyStamp entity)
                return;
            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        protected virtual void SetConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is IHasConcurrencyStamp entity)
                entity.ConcurrencyStamp ??= Guid.NewGuid().ToString("N");
        }

        protected void UpdateNavigationState(EntityEntry entry, EntityState state)
        {
            foreach (var navigationEntry in entry.Navigations)
                switch (navigationEntry)
                {
                    case ReferenceEntry referenceEntry:
                        var target = referenceEntry.TargetEntry;
                        if (target != null)
                        {
                            target.State = state;
                            UpdateNavigationState(target, state);
                        }

                        break;
                    case CollectionEntry collectionEntry:
                        foreach (var item in collectionEntry.CurrentValue ?? Enumerable.Empty<object>())
                        {
                            var entityEntry = collectionEntry.FindEntry(item);
                            if (entityEntry == null)
                                continue;
                            entityEntry.State = state;
                            UpdateNavigationState(entityEntry, state);
                        }

                        break;
                }
        }

        protected virtual async Task HandleEntityEntryAsync(EntityEntry entry, CancellationToken cancellationToken)
        {
            var entryEntity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    SetConcurrencyStamp(entryEntity);
                    _auditPropertySetter.SetCreationProperties(entryEntity);
                    _auditPropertySetter.SetModificationProperties(entryEntity);

                    break;

                case EntityState.Modified:
                    UpdateConcurrencyStamp(entryEntity);
                    _auditPropertySetter.SetModificationProperties(entryEntity);

                    break;
                case EntityState.Deleted:
                    UpdateConcurrencyStamp(entryEntity);
                    if (entryEntity is ISoftDelete && _dataFilter.IsEnabled<ISoftDelete>())
                    {
                        await entry.ReloadAsync(cancellationToken);
                        _auditPropertySetter.SetDeletionProperties(entryEntity);
                        entry.State = EntityState.Modified;
                        //The State of nested entity is still deleted;
                        UpdateNavigationState(entry, EntityState.Unchanged);
                    }

                    break;
            }
        }

        public virtual async Task BeforeSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entityEntries = ChangeTracker.Entries().ToList();
            foreach (var entry in entityEntries)
                await HandleEntityEntryAsync(entry, cancellationToken);

            var changeInfos = EntityChangeInfoUtil.GetChangeInfos(entityEntries);
            await _domainEventsDispatcher.DispatchEventsAsync(changeInfos, cancellationToken);
        }

        public virtual Task AfterSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            await BeforeSaveChangesAsync(cancellationToken);
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await AfterSaveChangesAsync(cancellationToken);
            return changes;
        }
    }
}