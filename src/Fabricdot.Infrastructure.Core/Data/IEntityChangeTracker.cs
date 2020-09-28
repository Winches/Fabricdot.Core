using System.Collections.Generic;
using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface IEntityChangeTracker
    {
        ICollection<IEntity<object>> Entries();
    }
}