using Fabricdot.Core.Modularity;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

[Requires(typeof(FabricdotMultiTenancyAbstractionModule))]
[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class FabricdotEntityFrameworkCoreModule : ModuleBase
{
}
