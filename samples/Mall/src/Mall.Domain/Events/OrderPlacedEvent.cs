using Fabricdot.Domain.Events;
using Mall.Domain.Aggregates.OrderAggregate;

namespace Mall.Domain.Events;

public class OrderPlacedEvent : EntityCreatedEvent<Order>
{
    public OrderPlacedEvent(Order entity) : base(entity)
    {
    }
}