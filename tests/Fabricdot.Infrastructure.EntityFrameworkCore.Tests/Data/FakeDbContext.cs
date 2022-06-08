using System.Reflection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

public class FakeDbContext : DbContextBase
{
    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<BookTag> BookTags { get; set; }

    public DbSet<BookContents> BookContents { get; set; }

    public DbSet<Employee> Employees { get; set; }

    /// <inheritdoc />
    public FakeDbContext([NotNull] DbContextOptions<FakeDbContext> options) : base(options)
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