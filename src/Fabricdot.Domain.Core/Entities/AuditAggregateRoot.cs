using System;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class AuditAggregateRoot<TKey> : CreationAuditAggregateRoot<TKey>, IAuditEntity
    {
        /// <inheritdoc />
        public DateTime LastModificationTime { get; protected set; }

        /// <inheritdoc />
        public string LastModifierId { get; protected set; }
    }
}