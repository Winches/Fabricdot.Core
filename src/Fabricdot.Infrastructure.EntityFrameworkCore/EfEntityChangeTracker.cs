using System.Collections.Generic;
using System.Linq;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Infrastructure.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class EfEntityChangeTracker : IEntityChangeTracker
    {
        private readonly DbContext _context;

        public EfEntityChangeTracker(DbContext context)
        {
            _context = context;
        }

        public ICollection<IEntity<object>> Entries()
        {
            var type = typeof(IEntity<>);
            var domainEntities = _context.ChangeTracker.Entries()
                .Select(v => v.Entity)
                .Where(v => v.GetType()
                    .GetInterfaces()
                    .Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == type))
                .Cast<IEntity<object>>()
                .ToList();
            return domainEntities;
        }
    }
}