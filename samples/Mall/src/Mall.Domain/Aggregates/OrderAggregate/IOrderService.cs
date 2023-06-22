using System;
using Fabricdot.Domain.Services;

namespace Mall.Domain.Aggregates.OrderAggregate;

public interface IOrderService : IDomainService
{
    Order Create(
        Address shippingAddress,
        Guid customerId);
}
