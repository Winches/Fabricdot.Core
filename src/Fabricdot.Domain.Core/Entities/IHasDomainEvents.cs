using System.Collections.Generic;
using Fabricdot.Domain.Core.Events;

namespace Fabricdot.Domain.Core.Entities
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void AddDomainEvent(IDomainEvent domainEvent);

        void RemoveDomainEvent(IDomainEvent domainEvent);

        void ClearDomainEvents();
    }
}