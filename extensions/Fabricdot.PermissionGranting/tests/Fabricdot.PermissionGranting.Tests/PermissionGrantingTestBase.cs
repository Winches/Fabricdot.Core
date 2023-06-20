using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.PermissionGranting.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests;

public abstract class PermissionGrantingTestBase : IntegrationTestBase<PermissionGrantingTestModule>
{
    protected PermissionGrantingTestBase()
    {
        ServiceProvider.GetRequiredService<FakeDataBuilder>()
               .BuildAsync()
               .GetAwaiter()
               .GetResult();
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }
}
