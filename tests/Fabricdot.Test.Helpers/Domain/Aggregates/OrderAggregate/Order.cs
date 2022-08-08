using Ardalis.GuardClauses;
using AutoFixture;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class Order : FullAuditAggregateRoot<Guid>
{
    private readonly List<OrderLine> _orderLines = new();

    public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

    public Money Total => _orderLines.Sum(v => v.Price * v.Quantity);

    public DateTime OrderTime { get; private set; }

    public OrderStatus OrderStatus { get; private set; }

    public Address ShippingAddress { get; private set; }

    public string CustomerId { get; private set; }

    public OrderDetails Details { get; set; }

    public Order(
        Guid id,
        Address shippingAddress,
        string customerId,
        OrderDetails details)
    {
        Id = Guard.Against.Default(id, nameof(id));
        ShippingAddress = Guard.Against.Null(shippingAddress, nameof(shippingAddress));
        CustomerId = Guard.Against.NullOrEmpty(customerId, nameof(customerId));
        OrderTime = SystemClock.Now;
        OrderStatus = OrderStatus.Placed;
        Details = details;
    }

    private Order()
    {
    }

    public void AddOrderLine(Guid productId, int quantity, Money price)
    {
        if (OrderStatus != OrderStatus.Placed)
            throw new DomainException("Order can not be modified.");

        var orderLine = _orderLines.SingleOrDefault(v => v.ProductId == productId);
        if (orderLine == null)
            _orderLines.Add(new OrderLine(Id, productId, quantity, price));
        else
            orderLine.AddQuantity(quantity);
    }

    public void RemoveOrderLine(Guid productId)
    {
        _orderLines.RemoveAll(v => v.ProductId == productId);
    }

    public void AddOrderLine(IFixture fixture)
    {
        AddOrderLine(
            fixture.Create<Guid>(),
            (int)fixture.Create<uint>(),
            Math.Abs(fixture.Create<decimal>()));
    }
}
