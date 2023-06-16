using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

internal class OrderConfiguration : EntityTypeConfigurationBase<Order>, IDbContextEntityConfiguration<FakeDbContext>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ConfigureEnumeration<OrderStatus>(nameof(Order.OrderStatus));

        builder.OwnsOne(v => v.ShippingAddress, b =>
        {
            b.WithOwner();
        });

        builder.Navigation(v => v.OrderLines)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(v => v.OrderLines)
               .WithOne()
               .HasForeignKey(v => v.OrderId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Details)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
