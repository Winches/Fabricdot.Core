using Fabricdot.Core.Modularity;

namespace Fabricdot.MultiTenancy.Tests.Strategies
{
    [Requires(typeof(FabricdotMultiTenancyModule))]
    [Exports]
    public class MultiTenancyTestModule : ModuleBase
    {
    }
}