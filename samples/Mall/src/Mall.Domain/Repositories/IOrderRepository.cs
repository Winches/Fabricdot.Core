using System;
using Fabricdot.Domain.Services;
using Mall.Domain.Aggregates.OrderAggregate;

namespace Mall.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, Guid>
{
}