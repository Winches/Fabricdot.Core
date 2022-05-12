using System;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fabricdot.Test.Shared
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected IServiceProviderFactory<IServiceCollection> ServiceProviderFactory { get; set; } = new DefaultServiceProviderFactory();
        protected IServiceProvider ServiceProvider { get; set; }
        protected IServiceScope ServiceScope { get; set; }

        protected IntegrationTestBase()
        {
            Initialize();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Initialize()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(v => v.AddProvider(NullLoggerProvider.Instance));
            ConfigureServices(serviceCollection);
            ServiceProvider = ServiceProviderFactory?.CreateServiceProvider(serviceCollection)
                ?? serviceCollection.BuildServiceProvider();
            ServiceScope = ServiceProvider.CreateScope();
        }

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);

        protected void UseServiceProviderFactory<TFactory>() where TFactory : IServiceProviderFactory<IServiceCollection>, new()
        {
            ServiceProviderFactory = new TFactory();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                ServiceScope?.Dispose();
        }
    }

    public abstract class IntegrationTestBase<TModule> where TModule : class, IModule
    {
        protected IServiceProviderFactory<IServiceCollection> ServiceProviderFactory { get; set; } = new DefaultServiceProviderFactory();
        protected IServiceProvider ServiceProvider { get; set; }
        protected IServiceScope ServiceScope { get; set; }

        protected IntegrationTestBase()
        {
            Initialize();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Initialize()
        {
            var services = new ServiceCollection();
            var app = services.AddBootstrapper<TModule>();
            ConfigureServices(services);

            ServiceProvider = ServiceProviderFactory?.CreateServiceProvider(services) ?? services.BuildServiceProvider();
            ServiceScope = ServiceProvider.CreateScope();

            app.Build(ServiceScope.ServiceProvider);
        }

        protected virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
        }

        protected void UseServiceProviderFactory<TFactory>() where TFactory : IServiceProviderFactory<IServiceCollection>, new()
        {
            ServiceProviderFactory = new TFactory();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                ServiceScope?.Dispose();
        }
    }
}