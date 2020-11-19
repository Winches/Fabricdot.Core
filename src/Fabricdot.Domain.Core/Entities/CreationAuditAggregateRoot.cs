using System;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class CreationAuditAggregateRoot<TKey> : AggregateRootBase<TKey>, ICreationAuditEntity
    {
        /// <inheritdoc />
        public DateTime CreationTime { get; protected set; }

        /// <inheritdoc />
        public string CreatorId { get; protected set; }
    }
}