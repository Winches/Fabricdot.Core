using System.Reflection;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Fabricdot.PermissionGranting.Domain;
using Fabricdot.PermissionGranting.Infrastructure.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.PermissionGranting.Tests.Data;

public class FakeDbContext : DbContextBase, IPermissionGrantingDbContext
{
    public DbSet<GrantedPermission> GrantedPermissions { get; set; }

    /// <inheritdoc />
    public FakeDbContext([NotNull] DbContextOptions<FakeDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ConfigurePermissionGranting();
    }
}