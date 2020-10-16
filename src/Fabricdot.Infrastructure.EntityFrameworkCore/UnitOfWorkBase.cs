using System;
using System.Threading.Tasks;
using Fabricdot.Common.Core.Enumerable;
using Fabricdot.Common.Core.Security;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using Fabricdot.Infrastructure.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class UnitOfWorkBase : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly ICurrentUser _currentUser;
        private readonly IDataFilter _filter;
        private bool _disposedValue;

        public UnitOfWorkBase(DbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _domainEventsDispatcher = serviceProvider.GetRequiredService<IDomainEventsDispatcher>();
            _currentUser = serviceProvider.GetRequiredService<ICurrentUser>();
            _filter = serviceProvider.GetRequiredService<IDataFilter>();
        }

        public virtual async Task<int> CommitChangesAsync()
        {
            var userid = _currentUser.Id;
            _context.ChangeTracker.Entries()
                .ForEach(v =>
                {
                    switch (v.State)
                    {
                        case EntityState.Added:
                            CreationAuditEntityInitializer.Init(v.Entity, userid);
                            ModificationAuditEntityInitializer.Init(v.Entity, userid);
                            break;

                        case EntityState.Modified:
                            ModificationAuditEntityInitializer.Init(v.Entity, userid);
                            break;
                        case EntityState.Deleted:
                            if (_filter.IsEnabled<ISoftDelete>() && v.Entity is ISoftDelete)
                            {
                                v.Reload();//It's will lose navigation property value without calling this method.
                                SoftDeleteEntityInitializer.Init(v.Entity);
                                v.State = EntityState.Modified;
                            }

                            break;
                    }
                });
            await _domainEventsDispatcher.DispatchEventsAsync();
            return await _context.SaveChangesAsync();
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