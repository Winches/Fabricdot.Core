using System;
using Fabricdot.Domain.Core.Auditing;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class FullAuditAggregateRoot<TKey> : AuditAggregateRoot<TKey>, IHasDeleterId, IHasDeletionTime
    {
        /// <inheritdoc />
        public bool IsDeleted { get; protected set; }

        /// <inheritdoc />
        public string DeleterId { get; protected set; }

        /// <inheritdoc />
        public DateTime? DeletionTime { get; protected set; }
    }
}