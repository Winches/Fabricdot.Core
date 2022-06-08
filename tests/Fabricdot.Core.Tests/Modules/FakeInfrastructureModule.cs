using Fabricdot.Core.Modularity;

namespace Fabricdot.Core.Tests.Modules;

[Requires(typeof(FakeCoreModule))]
internal class FakeInfrastructureModule : ModuleBase
{
}