using System;

namespace Fabricdot.Domain.Auditing;

public abstract class AuditEntity<TKey> : CreationAuditEntity<TKey>, IAuditEntity where TKey : notnull
{
    public string? LastModifierId { get; protected set; }

    public DateTime LastModificationTime { get; protected set; }
}