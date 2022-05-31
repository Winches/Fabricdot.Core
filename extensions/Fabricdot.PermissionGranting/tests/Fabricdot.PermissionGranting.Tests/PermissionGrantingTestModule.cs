using System.Data.Common;
using System.Diagnostics;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.Data;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests;

[Requires(typeof(FabricdotPermissionGrantingModule))]
[Exports]
public class PermissionGrantingTestModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;
        const string connectionString = "Filename=:memory:";
        services.Configure<DbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = connectionString;
        });
        var dbconnection = CreateInMemoryDatabase(connectionString);
        services.AddEfDbContext<FakeDbContext>((_, opts) =>
        {
            opts.UseSqlite(dbconnection);
            opts.LogTo(v => Debug.Print(v))
                .EnableSensitiveDataLogging();
        });
        services.AddPermissionGrantingStore<FakeDbContext>();
    }

    private static DbConnection CreateInMemoryDatabase(string connectionString)
    {
        var connection = new SqliteConnection(connectionString);
        connection.Open();
        using (var db = new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseSqlite(connection).Options))
        {
            db.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
        }

        return connection;
    }
}
