using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests;

public abstract class EntityFrameworkCoreTestsBase : IntegrationTestBase<EntityFrameworkCoreTestModule>
{
    protected EntityFrameworkCoreTestsBase()
    {
        var dataBuilder = ServiceProvider.GetRequiredService<FakeDataBuilder>();
        dataBuilder.BuildAsync().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }
}
