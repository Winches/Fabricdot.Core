using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class AuditAggregateRoot<TKey> : CreationAuditAggregateRoot<TKey>, IAuditEntity where TKey : notnull
{
    /// <inheritdoc />
    public DateTime? LastModificationTime { get; protected set; }

    /// <inheritdoc />
    public string? LastModifierId { get; protected set; }
}