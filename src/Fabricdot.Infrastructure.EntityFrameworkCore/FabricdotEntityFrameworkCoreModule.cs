using Fabricdot.Core.Modularity;
using Fabricdot.MultiTenancy;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

[Requires(typeof(FabricdotMultiTenancyAbstractionModule))]
[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class FabricdotEntityFrameworkCoreModule : ModuleBase
{
}
