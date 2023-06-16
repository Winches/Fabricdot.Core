using Fabricdot.Core.Modularity;
using Fabricdot.Core.Security;
using Fabricdot.Identity.Domain;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity;

[Requires(typeof(FabricdotEntityFrameworkCoreModule))]
[Requires(typeof(FabricdotIdentityDomainModule))]
[Exports]
public class FabricdotIdentityModule : ModuleBase
{
    public override void ConfigureServices(ConfigureServiceContext context)
    {
        context.Services.Configure<IdentityOptions>(options =>
        {
            var claimsIdentity = options.ClaimsIdentity;
            claimsIdentity.UserIdClaimType = SharedClaimTypes.NameIdentifier;
            claimsIdentity.UserNameClaimType = SharedClaimTypes.Name;
            claimsIdentity.RoleClaimType = SharedClaimTypes.Role;
            claimsIdentity.EmailClaimType = SharedClaimTypes.Email;
        });
    }
}
