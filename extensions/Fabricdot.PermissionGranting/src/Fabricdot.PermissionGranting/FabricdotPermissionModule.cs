using Fabricdot.Authorization;
using Fabricdot.Core.Modularity;
using Fabricdot.Domain;
using Fabricdot.Infrastructure.EntityFrameworkCore;

namespace Fabricdot.PermissionGranting;

[Requires(typeof(FabricdotDomainModule))]
[Requires(typeof(FabricdotAuthorizationModule))]
[Requires(typeof(FabricdotEntityFrameworkCoreModule))]
[Exports]
public class FabricdotPermissionGrantingModule : ModuleBase
{
}