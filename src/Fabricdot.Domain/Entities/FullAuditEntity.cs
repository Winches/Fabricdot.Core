using System;
using Fabricdot.Domain.Auditing;

namespace Fabricdot.Domain.Entities
{
    public abstract class FullAuditEntity<TKey> : AuditEntity<TKey>, IHasDeleterId, IHasDeletionTime
    {
        /// <inheritdoc />
        public bool IsDeleted { get; protected set; }

        /// <inheritdoc />
        public string DeleterId { get; protected set; }

        /// <inheritdoc />
        public DateTime? DeletionTime { get; protected set; }
    }
}