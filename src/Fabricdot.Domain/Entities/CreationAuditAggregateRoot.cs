using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class CreationAuditAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditEntity where TKey : notnull
{
    /// <inheritdoc />
    public virtual DateTime CreationTime { get; protected set; }

    /// <inheritdoc />
    public virtual string? CreatorId { get; protected set; }
}