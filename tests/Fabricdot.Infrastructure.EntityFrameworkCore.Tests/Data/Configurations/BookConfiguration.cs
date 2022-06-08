using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

public class BookConfiguration : EntityTypeConfigurationBase<Book>, IDbContextEntityConfiguration<FakeDbContext>
{
    public override void Configure(EntityTypeBuilder<Book> builder)
    {
        base.Configure(builder);

        builder.TryConfigureNavigationProperty(nameof(Book.Tags), PropertyAccessMode.Field);
        builder.HasMany(v => v.Tags)
               .WithOne()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(v => v.Contents)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}