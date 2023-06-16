using System.Data.Common;
using System.Diagnostics;
using Fabricdot.Core.Modularity;
using Fabricdot.Identity.Tests.Data;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.Data;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fabricdot.Identity.Tests;

[Requires(typeof(FabricdotIdentityModule))]
[Exports]
public class IdentityTestModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;
        var dbconnection = InMemoryDatabaseHelper.CreateConnection();
        CreateInMemoryDatabase(dbconnection);

        services.Configure<ConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = dbconnection.ConnectionString;
        });
        services.AddEfDbContext<FakeDbContext>((_, opts) =>
        {
            opts.UseSqlite(dbconnection);
            opts.LogTo(v => Debug.Print(v))
                .EnableSensitiveDataLogging();
        });

        services.AddIdentity<User, Role>()
                .AddRepositories<FakeDbContext>()
                .AddDefaultClaimsPrincipalFactory()
                .AddDefaultTokenProviders();

        var mockCurrentTenant = new Mock<ICurrentTenant>();
        mockCurrentTenant.Setup(v => v.Id).Returns((Guid?)null);
        services.AddSingleton(mockCurrentTenant.Object);
    }

    private static DbConnection CreateInMemoryDatabase(DbConnection connection)
    {
        using (var db = new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseSqlite(connection).Options))
        {
            db.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        return connection;
    }
}
