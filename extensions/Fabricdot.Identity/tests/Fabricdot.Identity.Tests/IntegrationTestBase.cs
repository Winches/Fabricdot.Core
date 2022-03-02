using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fabricdot.Identity.Tests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected IServiceProviderFactory<IServiceCollection> ServiceProviderFactory { get; set; }
        protected IServiceProvider ServiceProvider { get; set; }
        protected IServiceScope ServiceScope { get; set; }
        protected IServiceProvider ScopeServiceProvider => ServiceScope.ServiceProvider;

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

        protected void UseServiceProviderFactory(IServiceProviderFactory<IServiceCollection> serviceProviderFactory)
        {
            ServiceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        }

        protected void UseServiceProviderFactory<TFactory>() where TFactory : IServiceProviderFactory<IServiceCollection>, new()
        {
            UseServiceProviderFactory(new TFactory());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                ServiceScope?.Dispose();
        }
    }
}