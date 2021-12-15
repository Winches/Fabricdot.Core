using System;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Auditing
{
    public abstract class CreationAuditEntity<TKey> : EntityBase<TKey>, ICreationAuditEntity
    {
        public string CreatorId { get; protected set; }

        public DateTime CreationTime { get; protected set; }
    }
}