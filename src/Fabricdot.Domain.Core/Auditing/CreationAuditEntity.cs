﻿using System;
using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Auditing
{
    public abstract class CreationAuditEntity<TKey> : EntityBase<TKey>, ICreationAuditEntity
    {
        public string CreationId { get; protected set; }

        public DateTime CreationTime { get; protected set; }
    }
}