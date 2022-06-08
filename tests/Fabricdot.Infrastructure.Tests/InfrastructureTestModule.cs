using Fabricdot.Core.Modularity;

namespace Fabricdot.Infrastructure.Tests;

[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class InfrastructureTestModule : ModuleBase, IPostConfigureService
{
    public void PostConfigureServices(ConfigureServiceContext context)
    {
        //context.Services.AddInterceptors();
    }
}