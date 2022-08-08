using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

internal class OrderRepository : EfRepository<FakeDbContext, Order, Guid>, IOrderRepository
{
    /// <inheritdoc />
    public OrderRepository(IDbContextProvider<FakeDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}
