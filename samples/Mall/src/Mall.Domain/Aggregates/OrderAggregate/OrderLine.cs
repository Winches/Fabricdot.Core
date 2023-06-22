using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;

namespace Mall.Domain.Aggregates.OrderAggregate;

public class OrderLine : AuditEntity<Guid>
{
    public Guid ProductId { get; private set; }

    public int Quantity { get; private set; }

    public Money Price { get; private set; } = null!;

    internal OrderLine(
        Guid productId,
        int quantity,
        Money price)
    {
        ProductId = Guard.Against.Default(productId, nameof(productId));
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
