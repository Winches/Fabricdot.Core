using System.Reflection;
using Fabricdot.Identity.Infrastructure.Data;
using Fabricdot.Identity.Tests.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Tests.Data;

public class FakeDbContext : IdentityDbContext<User, Role>
{
    /// <inheritdoc />
    public FakeDbContext([NotNull] DbContextOptions<FakeDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
