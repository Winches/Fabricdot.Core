using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.Events;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        protected bool Equals(EntityBase<TKey> other)
        {
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((EntityBase<TKey>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// identity key
        /// </summary>
        public TKey Id { get; protected set; }

        /// <summary>
        /// events
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            Guard.Against.Null(domainEvent, nameof(domainEvent));

            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}