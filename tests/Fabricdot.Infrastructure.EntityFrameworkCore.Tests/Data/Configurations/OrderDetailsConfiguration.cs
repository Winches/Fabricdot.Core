using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

internal class OrderDetailsConfiguration : EntityTypeConfigurationBase<OrderDetails>, IDbContextEntityConfiguration<FakeDbContext>
{
}
