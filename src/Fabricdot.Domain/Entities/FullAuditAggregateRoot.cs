using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class FullAuditAggregateRoot<TKey> : AuditAggregateRoot<TKey>, IHasDeleterId, IHasDeletionTime where TKey : notnull
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; protected set; }

    /// <inheritdoc />
    public virtual string? DeleterId { get; protected set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; protected set; }
}
