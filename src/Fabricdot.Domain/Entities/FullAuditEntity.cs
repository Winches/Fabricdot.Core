using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class FullAuditEntity<TKey> : AuditEntity<TKey>, IHasDeleterId, IHasDeletionTime where TKey : notnull
{
    /// <inheritdoc />
    public virtual bool IsDeleted { get; protected set; }

    /// <inheritdoc />
    public virtual string? DeleterId { get; protected set; }

    /// <inheritdoc />
    public virtual DateTime? DeletionTime { get; protected set; }
}