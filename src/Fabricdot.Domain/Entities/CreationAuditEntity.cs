using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Auditing;

public abstract class CreationAuditEntity<TKey> : Entity<TKey>, ICreationAuditEntity where TKey : notnull
{
    public virtual string? CreatorId { get; protected set; }

    public virtual DateTime CreationTime { get; protected set; }
}
