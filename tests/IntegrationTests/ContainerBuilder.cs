using System;
using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using IntegrationTests.Data;
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
                _services.AddDbContext<FakeDbContext>(opts => { opts.UseInMemoryDatabase("TestAspCore"); });
                _services.AddScoped<IEntityChangeTracker, FakeEntityChangeTracker>();
                _services.AddScoped<IUnitOfWork, FakeUnitOfWork>();
                _services.AddScoped<IFakeRepository, FakeRepository>();
                _services.AddScoped<ICurrentUser, FakeCurrentUser>();
            }

            return _services;
        }
    }
}