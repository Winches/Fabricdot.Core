using System.Reflection;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mall.Infrastructure.Data
{
    public class AppDbContext : DbContextBase
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}