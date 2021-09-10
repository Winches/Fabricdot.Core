using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data
{
    public interface IDbContextEntityConfiguration<TDbContext> where TDbContext : DbContext
    {
    }
}