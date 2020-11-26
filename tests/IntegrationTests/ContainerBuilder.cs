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
        private static IServiceProvider _serviceProvider;
        private static IServiceCollection _services;

        public static IServiceProvider GetServiceProvider()
        {
            _serviceProvider ??= GetServices().BuildServiceProvider();
            return _serviceProvider;
        }

        public static IServiceCollection GetServices()
        {
            if (_services == null)
            {
                _services = new ServiceCollection();
                _services.RegisterModules(new InfrastructureModule());
                _services.AddDbContext<FakeDbContext>(opts =>
                {
                    opts.UseSqlite(CreateInMemoryDatabase());
                });
                _services.AddScoped<IEntityChangeTracker, FakeEntityChangeTracker>();
                _services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
                _services.AddScoped<IFakeRepository, FakeRepository>();
                _services.AddScoped<IBookRepository, BookRepository>();
                _services.AddScoped<ICurrentUser, FakeCurrentUser>();
                _services.AddTransient<IAuditPropertySetter, AuditPropertySetter>();
                _services.AddTransient<FakeDataBuilder>();
            }

            return _services;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }
    }
}