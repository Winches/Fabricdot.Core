using System.Reflection;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Mall.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Mall.Infrastructure.Data;

public class AppDbContext : DbContextBase
{
    public DbSet<Order> Orders => Set<Order>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}