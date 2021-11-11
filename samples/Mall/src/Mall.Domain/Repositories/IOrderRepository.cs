using System;
using Fabricdot.Domain.Core.Services;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order, Guid>
    {
    }
}