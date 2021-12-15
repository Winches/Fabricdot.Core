using System.Data.Common;
using Fabricdot.Infrastructure.Data;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestsBase : IntegrationTestBase
    {
        protected EntityFrameworkCoreTestsBase()
        {
            var provider = ServiceScope.ServiceProvider;
            var dataBuilder = provider.GetRequiredService<FakeDataBuilder>();
            dataBuilder.BuildAsync().GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            const string connectionString = "Filename=:memory:";
            serviceCollection.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = connectionString;
            });
            serviceCollection.RegisterModules(new InfrastructureModule());
            var dbconnection = CreateInMemoryDatabase(connectionString);
            serviceCollection.AddEfDbContext<FakeDbContext>((_, opts) =>
            {
                opts.UseSqlite(dbconnection);
            });
            serviceCollection.AddEfDbContext<FakeSecondDbContext>((_, opts) =>
            {
                opts.UseSqlite(dbconnection);
            });
            serviceCollection.AddTransient<FakeDataBuilder>();
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