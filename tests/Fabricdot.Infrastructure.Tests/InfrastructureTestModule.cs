using Fabricdot.Core.Modularity;

namespace Fabricdot.Infrastructure.Tests;

[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class InfrastructureTestModule : ModuleBase
{
}