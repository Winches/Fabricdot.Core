using System;
using Fabricdot.Domain.Core.Services;

namespace Mall.Domain.Entities.OrderAggregate
{
    public interface IOrderService : IDomainService
    {
        Order Create(
            Address shippingAddress,
            Guid customerId);
    }
}