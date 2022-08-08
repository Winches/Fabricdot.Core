using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities;

public abstract class AuditEntity<TKey> : CreationAuditEntity<TKey>, IAuditEntity where TKey : notnull
{
    public virtual string? LastModifierId { get; protected set; }

    public virtual DateTime? LastModificationTime { get; protected set; }
}