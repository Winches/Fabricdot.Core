using Fabricdot.Core.Modularity;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    [Requires(typeof(FabricdotInfrastructureModule))]
    [Exports]
    public class FabricdotEntityFrameworkCoreModule : ModuleBase
    {
    }
}