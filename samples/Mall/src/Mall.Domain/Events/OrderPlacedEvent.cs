using Fabricdot.Domain.Core.Events;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.Domain.Events
{
    public class OrderPlacedEvent : EntityCreatedEvent<Order>
    {
        public OrderPlacedEvent(Order entity) : base(entity)
        {
        }
    }
}