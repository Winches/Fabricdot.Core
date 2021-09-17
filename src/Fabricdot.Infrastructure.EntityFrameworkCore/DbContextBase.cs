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

        protected IDataFilter DataFilter => _dataFilter ??= this.GetRequiredService<IDataFilter>();
        protected IAuditPropertySetter AuditPropertySetter => _auditPropertySetter ??= this.GetRequiredService<IAuditPropertySetter>();
        protected IDomainEventsDispatcher DomainEventsDispatcher => _domainEventsDispatcher ??= this.GetRequiredService<IDomainEventsDispatcher>();

        /// <inheritdoc />
        protected DbContextBase()
        {
            Initialize();
        }

        /// <inheritdoc />
        protected DbContextBase([NotNull] DbContextOptions options) : base(options)
        {
            Initialize();
        }

        public virtual async Task BeforeSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entityEntries = ChangeTracker.Entries().ToList();
            var changeInfos = EntityChangeInfoUtil.GetChangeInfos(entityEntries);
            await DomainEventsDispatcher.DispatchEventsAsync(changeInfos, cancellationToken);

            foreach (var entry in ChangeTracker.Entries())
                await HandleEntityEntryAsync(entry, cancellationToken);
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

        protected void Initialize()
        {
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.Immediate;
        }

        protected virtual void UpdateConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is not IHasConcurrencyStamp entity)
                return;
            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = NewConcurrencyStamp();
        }

        protected virtual void SetConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is IHasConcurrencyStamp entity)
                entity.ConcurrencyStamp ??= NewConcurrencyStamp();
        }

        protected virtual async Task HandleEntityEntryAsync(EntityEntry entry, CancellationToken cancellationToken)
        {
            var entryEntity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    SetConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetCreationProperties(entryEntity);
                    AuditPropertySetter.SetModificationProperties(entryEntity);

                    break;

                case EntityState.Modified:
                    UpdateConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetModificationProperties(entryEntity);

                    break;

                case EntityState.Deleted:
                    if (!ShouldBeDeleteSoftly(entryEntity))
                        break;

                    await entry.ReloadAsync(cancellationToken);
                    UpdateConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetDeletionProperties(entryEntity);
                    entry.State = EntityState.Modified;

                    break;
            }
        }

        //protected void UpdateNavigationState(EntityEntry entry, EntityState state)
        //{
        //    foreach (var navigationEntry in entry.Navigations)
        //        switch (navigationEntry)
        //        {
        //            case ReferenceEntry referenceEntry:
        //                var target = referenceEntry.TargetEntry;
        //                if (target != null)
        //                {
        //                    target.State = state;
        //                    UpdateNavigationState(target, state);
        //                }
        //                break;
        //            case CollectionEntry collectionEntry:
        //                foreach (var item in collectionEntry.CurrentValue ?? Enumerable.Empty<object>())
        //                {
        //                    var entityEntry = collectionEntry.FindEntry(item);
        //                    if (entityEntry == null)
        //                        continue;
        //                    entityEntry.State = state;
        //                    UpdateNavigationState(entityEntry, state);
        //                }
        //                break;
        //        }
        //}
        protected bool ShouldBeDeleteSoftly(object entity) => entity is ISoftDelete && DataFilter.IsEnabled<ISoftDelete>();

        private static string NewConcurrencyStamp() => Guid.NewGuid().ToString("N");
    }
}