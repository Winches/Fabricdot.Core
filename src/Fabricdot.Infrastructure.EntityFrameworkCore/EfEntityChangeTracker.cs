using System.Collections.Generic;
using System.Linq;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class EfEntityChangeTracker : IEntityChangeTracker
    {
        private readonly DbContext _context;

        public EfEntityChangeTracker(DbContext context)
        {
            _context = context;
        }

        public ICollection<EntityChangeInfo> Entries()
        {
            var type = typeof(IEntity<>);
            var domainEntities = _context.ChangeTracker.Entries()
                .Select(CreateEntityChangeInfo)
                .Where(v => v != null && v.Entity.GetType()
                    .GetInterfaces()
                    .Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == type))
                .ToList();
            return domainEntities;
        }

        private static EntityChangeInfo CreateEntityChangeInfo(EntityEntry entry)
        {
            EntityStatus entityStatus;
            switch (entry.State)
            {
                case EntityState.Added:
                    entityStatus = EntityStatus.Created;
                    break;
                case EntityState.Modified:
                    entityStatus = EntityStatus.Updated;
                    break;
                case EntityState.Deleted:
                    entityStatus = EntityStatus.Deleted;
                    break;
                default:
                    return null;
            }

            return new EntityChangeInfo(entry.Entity, entityStatus);
        }
    }
}