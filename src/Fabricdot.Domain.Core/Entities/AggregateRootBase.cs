﻿using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Events;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class AggregateRootBase<TKey> : EntityBase<TKey>, IAggregateRoot, IHasDomainEvents,
        IHasConcurrencyStamp
    {
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        /// <summary>
        ///     events
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <inheritdoc />
        public string ConcurrencyStamp { get; set; }

        protected AggregateRootBase()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}