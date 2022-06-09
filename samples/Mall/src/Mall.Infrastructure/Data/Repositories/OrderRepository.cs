using System;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Mall.Domain.Aggregates.OrderAggregate;
using Mall.Domain.Repositories;

namespace Mall.Infrastructure.Data.Repositories;

internal class OrderRepository : EfRepository<AppDbContext, Order, Guid>, IOrderRepository
{
    public OrderRepository(IDbContextProvider<AppDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}