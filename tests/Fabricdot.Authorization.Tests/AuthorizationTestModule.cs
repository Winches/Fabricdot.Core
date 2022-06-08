using Fabricdot.Core.Modularity;

namespace Fabricdot.Authorization.Tests;

[Requires(typeof(FabricdotAuthorizationModule))]
[Exports]
public class AuthorizationTestModule : ModuleBase
{
}