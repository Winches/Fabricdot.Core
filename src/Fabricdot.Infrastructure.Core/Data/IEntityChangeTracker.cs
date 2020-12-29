using System.Collections.Generic;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface IEntityChangeTracker
    {
        ICollection<EntityChangeInfo> Entries();
    }
}