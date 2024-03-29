using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

internal class OrderConfiguration : EntityTypeConfigurationBase<Order>, IDbContextEntityConfiguration<FakeDbContext>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.Property(v => v.OrderStatus)
               .IsEnumeration()
               .IsRequired();

        builder.OwnsOne(v => v.ShippingAddress, b => b.WithOwner());

        builder.Navigation(v => v.OrderLines)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(v => v.CustomerId).IsTypedKey();

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
