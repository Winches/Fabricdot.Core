using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Mall.Domain.Entities.OrderAggregate;
using Mall.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mall.Infrastructure.Data.Configuration
{
    internal class OrderLineConfiguration : EntityTypeConfigurationBase<OrderLine>
    {
        public override void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            base.Configure(builder);
            builder.ToTable(Schema.OrderLine);

            builder.Property(v => v.Quantity)
                .IsRequired();

            builder.Property(v => v.Price)
                .HasConversion(v => v.Value, v => v)
                .HasPrecision(MonyConstant.Precision, MonyConstant.Scale)
                .IsRequired();
        }
    }
}