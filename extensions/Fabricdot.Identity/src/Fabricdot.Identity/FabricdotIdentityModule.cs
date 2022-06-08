using Fabricdot.Core.Modularity;
using Fabricdot.Identity.Domain;
using Fabricdot.Infrastructure.EntityFrameworkCore;

namespace Fabricdot.Identity;

[Requires(typeof(FabricdotEntityFrameworkCoreModule))]
[Requires(typeof(FabricdotIdentityDomainModule))]
[Exports]
public class FabricdotIdentityModule : ModuleBase
{
}