using Fabricdot.Domain.Events;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class OrderPlacedDomainEvent : DomainEventBase
{
    public Guid OrderId { get; }

    public OrderPlacedDomainEvent(Guid orderId)
    {
        OrderId = orderId;
    }
}
