using System.Collections.Generic;
using System.Linq;
using Fabricdot.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public static class EntityChangeInfoUtil
    {
        public static ICollection<EntityChangeInfo> GetChangeInfos(IEnumerable<EntityEntry> entries)
        {
            return entries
                .Select(GetChangeInfo)
                .Where(v => v != null)
                .ToList();
        }

        private static EntityChangeInfo GetChangeInfo(EntityEntry entry)
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