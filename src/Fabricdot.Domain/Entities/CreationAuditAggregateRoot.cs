using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities
{
    public abstract class CreationAuditAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditEntity
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; protected set; }

        /// <inheritdoc />
        public string CreatorId { get; protected set; }
    }
}