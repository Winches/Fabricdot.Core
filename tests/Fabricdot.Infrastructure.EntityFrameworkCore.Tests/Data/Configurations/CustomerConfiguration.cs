using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

public class CustomerConfiguration : EntityTypeConfigurationBase<Customer>, IDbContextEntityConfiguration<FakeSecondDbContext>
{
}