using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public abstract class EntityFrameworkCoreTestsBase : IntegrationTestBase<EntityFrameworkCoreTestModule>
    {
        protected EntityFrameworkCoreTestsBase()
        {
            var provider = ServiceScope.ServiceProvider;
            var dataBuilder = provider.GetRequiredService<FakeDataBuilder>();
            dataBuilder.BuildAsync().GetAwaiter().GetResult();
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            UseServiceProviderFactory<FabricdotServiceProviderFactory>();
        }
    }
}