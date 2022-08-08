using Fabricdot.Core.Boot;
using Fabricdot.Core.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.Modules;

[Requires(typeof(FakeCoreModule))]
[Requires(typeof(FakeInfrastructureModule))]
[Exports]
internal class FakeStartupModule : ModuleBase, IPreConfigureService, IPostConfigureService
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        context.Services.Should().NotBeNull();
        context.Configuration.Should().NotBeNull();
        base.ConfigureServices(context);
    }

    public override Task OnStartingAsync(ApplicationStartingContext context)
    {
        context.ServiceProvider.GetService<IConfiguration>().Should().NotBeNull();
        return base.OnStartingAsync(context);
    }

    public override Task OnStoppingAsync(ApplicationStoppingContext context)
    {
        context.ServiceProvider.GetService<IConfiguration>().Should().NotBeNull();
        return base.OnStoppingAsync(context);
    }

    public void PreConfigureServices(ConfigureServiceContext context)
    {
    }

    public void PostConfigureServices(ConfigureServiceContext context)
    {
    }
}