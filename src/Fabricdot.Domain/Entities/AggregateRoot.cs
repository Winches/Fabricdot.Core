using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Events;

namespace Fabricdot.Domain.Entities
{
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot, IHasDomainEvents, IHasConcurrencyStamp
        where TKey : notnull
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        /// <summary>
        ///     events
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <inheritdoc />
        public string ConcurrencyStamp { get; set; }

        protected AggregateRoot()
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