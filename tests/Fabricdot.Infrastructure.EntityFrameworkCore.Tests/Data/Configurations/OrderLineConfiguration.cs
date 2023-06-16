using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

internal class OrderLineConfiguration : EntityTypeConfigurationBase<OrderLine>, IDbContextEntityConfiguration<FakeDbContext>
{
    public override void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        base.Configure(builder);

        builder.Property(v => v.Price).HasConversion(v => v.Value, v => v);
    }
}
