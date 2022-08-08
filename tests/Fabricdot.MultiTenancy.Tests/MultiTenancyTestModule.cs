using Fabricdot.Core.Modularity;

namespace Fabricdot.MultiTenancy.Tests;

[Requires(typeof(FabricdotMultiTenancyModule))]
[Exports]
public class MultiTenancyTestModule : ModuleBase
{
}