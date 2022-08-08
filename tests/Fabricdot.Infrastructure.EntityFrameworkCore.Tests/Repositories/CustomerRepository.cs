using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

internal class CustomerRepository : EfRepository<FakeSecondDbContext, Customer, string>, ICustomerRepository
{
    /// <inheritdoc />
    public CustomerRepository(IDbContextProvider<FakeSecondDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}