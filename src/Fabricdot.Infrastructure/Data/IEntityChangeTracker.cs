using System.Collections.Generic;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Infrastructure.Data;

public interface IEntityChangeTracker
{
    ICollection<EntityChangeInfo> Entries();
}