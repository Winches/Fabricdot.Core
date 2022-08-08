using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static readonly OrderStatus Placed = new(1, nameof(Placed).ToLowerInvariant());

    public static readonly OrderStatus Shipped = new(2, nameof(Shipped).ToLowerInvariant());

    public static readonly OrderStatus Completed = new(3, nameof(Completed).ToLowerInvariant());

    public OrderStatus(
        int value,
        string name) : base(value, name)
    {
    }

    public static implicit operator OrderStatus(int value) => FromValue<OrderStatus>(value);
}
