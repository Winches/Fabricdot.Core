using Fabricdot.Domain.ValueObjects;

namespace Mall.Domain.Aggregates.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static OrderStatus Placed = new(1, nameof(Placed).ToLowerInvariant());

    public static OrderStatus Shipped = new(2, nameof(Shipped).ToLowerInvariant());

    public static OrderStatus Completed = new(3, nameof(Completed).ToLowerInvariant());

    protected OrderStatus(
        int value,
        string name) : base(value, name)
    {
    }

    public static implicit operator OrderStatus(int value) => FromValue<OrderStatus>(value);
}
