using Fabricdot.Core.Modularity;

namespace Fabricdot.Authorization;

[Requires(typeof(FabricdotAuthorizationAbstractionsModule))]
//[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class FabricdotAuthorizationModule : ModuleBase
{
}