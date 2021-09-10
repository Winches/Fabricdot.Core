using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations
{

    public class PublisherConfiguration : EntityTypeConfigurationBase<Publisher>, IDbContextEntityConfiguration<FakeSecondDbContext>
    {
    }
}