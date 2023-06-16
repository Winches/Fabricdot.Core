using System.Data.Common;
using System.Diagnostics;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

[Requires(typeof(FabricdotEntityFrameworkCoreModule))]
[Exports]
public class EntityFrameworkCoreTestModule : ModuleBase
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
        services.AddEfDbContext<FakeSecondDbContext>((_, opts) =>
        {
            opts.UseSqlite(dbconnection);
        });
    }

    private static DbConnection CreateInMemoryDatabase(DbConnection connection)
    {
        using (var db = new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseSqlite(connection).Options))
        {
            //db.Database.EnsureCreated();
            db.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
        }
        using (var secondDb = new FakeSecondDbContext(new DbContextOptionsBuilder<FakeSecondDbContext>().UseSqlite(connection).Options))
        {
            //secondDb.Database.EnsureCreated();
            secondDb.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        return connection;
    }
}
