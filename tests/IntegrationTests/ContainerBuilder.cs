using System;
using System.Data.Common;
using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Domain.Auditing;
using IntegrationTests.Data;
using IntegrationTests.Data.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public static class ContainerBuilder
    {
        private static readonly Lazy<IServiceCollection> Services = new Lazy<IServiceCollection>(CreateServices);
        private static readonly Lazy<IServiceProvider> ServiceProvider = new Lazy<IServiceProvider>(Services.Value.BuildServiceProvider);

        private static IServiceCollection CreateServices()
        {
            var services = new ServiceCollection();
            services.RegisterModules(new InfrastructureModule());
            services.AddDbContext<FakeDbContext>(opts =>
            {
                opts.UseSqlite(CreateInMemoryDatabase());
            });
            services.AddScoped<IEntityChangeTracker, FakeEntityChangeTracker>();
            services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ICurrentUser, FakeCurrentUser>();
            services.AddTransient<IAuditPropertySetter, AuditPropertySetter>();
            services.AddTransient<FakeDataBuilder>();
            return services;
        }

        public static IServiceProvider GetServiceProvider()
        {
            return ServiceProvider.Value;
        }

        public static IServiceCollection GetServices()
        {
            return Services.Value;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
    }
}