//using System.Data.Common;
//using System.Diagnostics;
//using AspectCore.Extensions.DependencyInjection;
//using Fabricdot.Identity.Tests.Data;
//using Fabricdot.Infrastructure.Data;
//using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.DependencyInjection;

//namespace Fabricdot.Identity.Tests
//{
//    public abstract class EntityFrameworkCoreTestsBase : IntegrationTestBase
//    {
//        protected EntityFrameworkCoreTestsBase()
//        {
//            var provider = ServiceScope.ServiceProvider;
//            var dataBuilder = provider.GetRequiredService<FakeDataBuilder>();
//            dataBuilder.BuildAsync().GetAwaiter().GetResult();
//        }

//        /// <inheritdoc />
//        protected override void ConfigureServices(IServiceCollection serviceCollection)
//        {
//            const string connectionString = "Filename=:memory:";
//            serviceCollection.Configure<DbConnectionOptions>(options =>
//            {
//                options.ConnectionStrings.Default = connectionString;
//            });
//            //serviceCollection.RegisterModules(new InfrastructureModule());
//            var dbconnection = CreateInMemoryDatabase(connectionString);
//            serviceCollection.AddEfDbContext<FakeDbContext>((_, opts) =>
//            {
//                opts.UseSqlite(dbconnection);
//                opts.LogTo(v => Debug.Print(v))
//                    .EnableSensitiveDataLogging();
//            });
//            serviceCollection.AddTransient<FakeDataBuilder>();
//            serviceCollection.AddInterceptors();
//            UseServiceProviderFactory<DynamicProxyServiceProviderFactory>();
//        }

//        private static DbConnection CreateInMemoryDatabase(string connectionString)
//        {
//            var connection = new SqliteConnection(connectionString);
//            connection.Open();
//            using (var db = new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseSqlite(connection).Options))
//            {
//                //db.Database.EnsureCreated();
//                db.Database.GetService<IRelationalDatabaseCreator>().CreateTables();
//            }

//            return connection;
//        }
//    }
//}