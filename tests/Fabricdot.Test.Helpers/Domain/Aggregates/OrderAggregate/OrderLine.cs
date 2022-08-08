using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class OrderLine : FullAuditEntity<int>
{
    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public Money Price { get; private set; }

    internal OrderLine(
        Guid orderId,
        Guid productId,
        int quantity,
        Money price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity));
        Price = Guard.Against.Null(price, nameof(price));
    }

    private OrderLine()
    {
    }

    internal void AddQuantity(int quantity)
    {
        quantity = Quantity + quantity;
        Quantity = quantity >= 0 ? quantity : 0;
    }
}