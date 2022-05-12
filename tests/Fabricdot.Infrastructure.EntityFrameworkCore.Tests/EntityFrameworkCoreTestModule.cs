using System.Data.Common;
using System.Diagnostics;
using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    [Requires(typeof(FabricdotEntityFrameworkCoreModule))]
    [Exports]
    public class EntityFrameworkCoreTestModule : ModuleBase
    {
        public override void ConfigureServices(ConfigureServiceContext context)
        {
            const string connectionString = "Filename=:memory:";
            var services = context.Services;

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
            services.AddEfDbContext<FakeSecondDbContext>((_, opts) =>
            {
                opts.UseSqlite(dbconnection);
            });
            services.AddTransient<FakeDataBuilder>();
        }

        private static DbConnection CreateInMemoryDatabase(string connectionString)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
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
}