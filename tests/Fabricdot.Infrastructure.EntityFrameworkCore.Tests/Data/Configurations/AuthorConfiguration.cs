using Fabricdot.Infrastructure.EntityFrameworkCore.Configurations;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data.Configurations;

public class AuthorConfiguration : EntityTypeConfigurationBase<Author>, IDbContextEntityConfiguration<FakeDbContext>
{
}