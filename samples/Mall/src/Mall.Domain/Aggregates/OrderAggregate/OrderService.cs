using System;
using Fabricdot.Core.UniqueIdentifier;

namespace Mall.Domain.Aggregates.OrderAggregate;

internal class OrderService : IOrderService
{
    private readonly IGuidGenerator _guidGenerator;

    public OrderService(IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }

    public Order Create(
        Address shippingAddress,
        Guid customerId)
    {
        var id = _guidGenerator.Create();
        return new Order(id, shippingAddress, customerId);
    }
}
