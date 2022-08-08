using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class AuditAggregateRoot<TKey> : CreationAuditAggregateRoot<TKey>, IAuditEntity where TKey : notnull
{
    /// <inheritdoc />
    public virtual DateTime? LastModificationTime { get; protected set; }

    /// <inheritdoc />
    public virtual string? LastModifierId { get; protected set; }
}