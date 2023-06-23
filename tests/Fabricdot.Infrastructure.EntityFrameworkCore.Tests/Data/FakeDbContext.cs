using System.Reflection;
using Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

public class FakeDbContext : DbContextBase
{
    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();

    /// <inheritdoc />
    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            v => v.IsAssignableTo(typeof(IDbContextEntityConfiguration<FakeDbContext>)));
    }
}
