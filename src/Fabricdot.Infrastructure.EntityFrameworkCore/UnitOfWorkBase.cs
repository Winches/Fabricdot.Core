using System;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class UnitOfWorkBase : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        //private readonly ICurrentUser _currentUser;
        private readonly IDataFilter _filter;
        private readonly IAuditPropertySetter _auditPropertySetter;
        private bool _disposedValue;

        public UnitOfWorkBase(DbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _domainEventsDispatcher = serviceProvider.GetRequiredService<IDomainEventsDispatcher>();
            //_currentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _filter = serviceProvider.GetRequiredService<IDataFilter>();
            _auditPropertySetter = serviceProvider.GetRequiredService<IAuditPropertySetter>();
        }

        public virtual async Task<int> CommitChangesAsync()
        {
            await _domainEventsDispatcher.DispatchEventsAsync();
            //var userid = _currentUser.Id;
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //CreationAuditEntityInitializer.Init(entry.Entity, userid);
                        //ModificationAuditEntityInitializer.Init(entry.Entity, userid);
                        _auditPropertySetter.SetCreationProperties(entry.Entity);
                        _auditPropertySetter.SetModificationProperties(entry.Entity);

                        break;

                    case EntityState.Modified:
                        //ModificationAuditEntityInitializer.Init(entry.Entity, userid);
                        _auditPropertySetter.SetCreationProperties(entry.Entity);

                        break;
                    case EntityState.Deleted:
                        if (_filter.IsEnabled<ISoftDelete>())
                        {
                            //The State of nested entity is still deleted;
                            if (entry.Entity is ISoftDelete)
                            {
                                await entry.ReloadAsync();
                                //SoftDeleteEntityInitializer.Init(entry.Entity);
                                _auditPropertySetter.SetDeletionProperties(entry.Entity);
                                entry.State = EntityState.Modified;
                                UpdateNavigationState(entry, EntityState.Unchanged);
                            }
                        }

                        break;
                }
            }
            return await _context.SaveChangesAsync();
        }

        protected void UpdateNavigationState(EntityEntry entry, EntityState state)
        {
            foreach (var navigationEntry in entry.Navigations)
            {
                switch (navigationEntry)
                {
                    case ReferenceEntry referenceEntry:
                        var target = referenceEntry.TargetEntry;
                        target.State = state;
                        UpdateNavigationState(target, state);
                        break;

                }
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