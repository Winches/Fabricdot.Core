﻿using System;

namespace Fabricdot.Domain.Core.Auditing
{
    public abstract class AuditEntity<TKey> : CreationAuditEntity<TKey>, IAuditEntity
    {
        public string LastModifierId { get; protected set; }

        public DateTime LastModificationTime { get; protected set; }
    }
}