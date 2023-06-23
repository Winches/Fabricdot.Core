using System.Reflection;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

public class FakeSecondDbContext : DbContextBase
{
    public DbSet<Customer> Customers => Set<Customer>();

    /// <inheritdoc />
    public FakeSecondDbContext(DbContextOptions<FakeSecondDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            v => v.IsAssignableTo(typeof(IDbContextEntityConfiguration<FakeSecondDbContext>)));
    }
}
