using Fabricdot.Domain.Core.ValueObjects;

namespace Mall.Domain.Entities.OrderAggregate
{
    public class OrderStatus : Enumeration
    {
        public static OrderStatus Placed = new OrderStatus(1, nameof(Placed).ToLowerInvariant());

        public static OrderStatus Shipped = new OrderStatus(2, nameof(Shipped).ToLowerInvariant());

        public static OrderStatus Completed = new OrderStatus(3, nameof(Completed).ToLowerInvariant());

        protected OrderStatus(int value, string name) : base(value, name)
        {
        }

        public static implicit operator OrderStatus(int value) => FromValue<OrderStatus>(value);
    }
}