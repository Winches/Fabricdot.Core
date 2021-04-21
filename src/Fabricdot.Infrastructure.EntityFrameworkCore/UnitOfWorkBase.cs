using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    [Obsolete("conflict with 'DbContextBase'")]
    public class UnitOfWorkBase : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly IDataFilter _filter;
        private readonly IAuditPropertySetter _auditPropertySetter;
        private bool _disposedValue;

        public UnitOfWorkBase(DbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _domainEventsDispatcher = serviceProvider.GetRequiredService<IDomainEventsDispatcher>();
            _filter = serviceProvider.GetRequiredService<IDataFilter>();
            _auditPropertySetter = serviceProvider.GetRequiredService<IAuditPropertySetter>();
        }

        public virtual async Task CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            //todo:consider override DbContext method
            var entries = _context.ChangeTracker.Entries().ToList();
            var changeInfos = EntityChangeInfoUtil.GetChangeInfos(entries);
            await _domainEventsDispatcher.DispatchEventsAsync(changeInfos, cancellationToken);
            foreach (var entry in entries)
                switch (entry.State)
                {
                    case EntityState.Added:
                        SetConcurrencyStamp(entry);
                        _auditPropertySetter.SetCreationProperties(entry.Entity);
                        _auditPropertySetter.SetModificationProperties(entry.Entity);

                        break;

                    case EntityState.Modified:
                        UpdateConcurrencyStamp(entry);
                        _auditPropertySetter.SetModificationProperties(entry.Entity);

                        break;
                    case EntityState.Deleted:
                        UpdateConcurrencyStamp(entry);
                        if (_filter.IsEnabled<ISoftDelete>())
                            //The State of nested entity is still deleted;
                            if (entry.Entity is ISoftDelete)
                            {
                                await entry.ReloadAsync(cancellationToken);
                                _auditPropertySetter.SetDeletionProperties(entry.Entity);
                                entry.State = EntityState.Modified;
                                UpdateNavigationState(entry, EntityState.Unchanged);
                            }

                        break;
                }

            await _context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
        {
            if (entry.Entity is IHasConcurrencyStamp entity)
            {
                _context.Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
                entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            }
        }

        protected virtual void SetConcurrencyStamp(EntityEntry entry)
        {
            if (entry.Entity is IHasConcurrencyStamp entity)
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    //dispose managed state (managed objects)
                    _context.Dispose();

                _disposedValue = true;
            }
        }
    }
}