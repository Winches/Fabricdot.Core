using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fabricdot.Test.Shared
{
    public abstract class IntegrationTestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider;
        protected IServiceScope ServiceScope;

        protected IntegrationTestBase()
        {
            Initialize();
        }

        protected void Initialize()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(v => v.AddProvider(NullLoggerProvider.Instance));
            ConfigureServices(serviceCollection);

            if (ServiceProvider != null)
                return;
            ServiceProvider = serviceCollection.BuildServiceProvider();
            ServiceScope = ServiceProvider.CreateScope();
        }

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                ServiceScope?.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}