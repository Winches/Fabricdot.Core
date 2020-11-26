using System;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Data;
using IntegrationTests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public abstract class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider;
        protected IServiceScope ServiceScope;

        protected IUnitOfWork UnitOfWork;

        protected TestBase()
        {
            ServiceProvider = ContainerBuilder.GetServiceProvider();
            ServiceScope = ServiceProvider.CreateScope();
            Initialize().GetAwaiter().GetResult();
        }

        protected async Task Initialize()
        {
            var provider = ServiceScope.ServiceProvider;
            var dataBuilder = provider.GetRequiredService<FakeDataBuilder>();
            await dataBuilder.BuildAsync();
            UnitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServiceScope?.Dispose();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}