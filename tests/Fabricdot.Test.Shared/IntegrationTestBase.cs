using System;
using Microsoft.Extensions.DependencyInjection;

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
            ConfigureServices(serviceCollection);
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