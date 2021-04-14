using System.Data.Common;
using Fabricdot.Infrastructure.Core;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Shared;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestsBase : IntegrationTestBase
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly FakeDbContext DbContext;

        protected EntityFrameworkCoreTestsBase()
        {
            var provider = ServiceScope.ServiceProvider;
            var dataBuilder = provider.GetRequiredService<FakeDataBuilder>();
            dataBuilder.BuildAsync().GetAwaiter().GetResult();
            UnitOfWork = provider.GetRequiredService<IUnitOfWork>();
            DbContext = provider.GetRequiredService<FakeDbContext>();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
            serviceCollection.AddDbContext<FakeDbContext>(opts =>
            {
                opts.UseSqlite(CreateInMemoryDatabase());
            });
            serviceCollection
                .AddScoped<IEntityChangeTracker, FakeEntityChangeTracker>()
                .AddScoped<IUnitOfWork, FakeUnitOfWork>();
            serviceCollection.AddTransient<FakeDataBuilder>();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
    }
}