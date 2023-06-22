using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Mall.Domain.Aggregates.OrderAggregate;
using Mall.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mall.Infrastructure.Data.Configuration;

internal class OrderConfiguration : EntityTypeConfigurationBase<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable(Schema.Order);

        builder.Property(v => v.Total)
            .HasConversion(v => v.Value, v => v)
            .HasPrecision(MonyConstant.Precision, MonyConstant.Scale)
            .IsRequired();

        builder.Property(v => v.OrderTime)
            .IsRequired();

        builder.ConfigureEnumeration<OrderStatus>(nameof(Order.OrderStatus))
            .IsRequired();

        builder.OwnsOne(v => v.ShippingAddress, b =>
        {
            b.WithOwner();

            b.Property(v => v.Country)
             .HasMaxLength(AddressConstant.CountryLength)
             .IsRequired();

            b.Property(v => v.State)
             .HasMaxLength(AddressConstant.StateLength)
             .IsRequired();

            b.Property(v => v.City)
             .HasMaxLength(AddressConstant.CityLength)
             .IsRequired();

            b.Property(v => v.Street)
             .HasMaxLength(AddressConstant.StreetLength)
             .IsRequired();
        });
        builder.Navigation(v => v.ShippingAddress)
            .IsRequired();

        builder.TryConfigureNavigationProperty(nameof(OrderLine), PropertyAccessMode.Field);
        builder.HasMany(v => v.OrderLines)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
