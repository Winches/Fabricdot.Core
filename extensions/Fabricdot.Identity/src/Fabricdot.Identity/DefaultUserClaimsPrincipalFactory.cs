using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fabricdot.Identity
{
    public class DefaultUserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory<TUser, TRole>
            where TUser : IdentityUser where TRole : IdentityRole
    {
        public DefaultUserClaimsPrincipalFactory(
            UserManager<TUser> userManager,
            RoleManager<TRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();

            if (!string.IsNullOrWhiteSpace(user.GivenName))
            {
                identity.GetOrAdd(ClaimTypes.GivenName, user.GivenName);
            }

            if (!string.IsNullOrWhiteSpace(user.Surname))
            {
                identity.GetOrAdd(ClaimTypes.Surname, user.Surname);
            }

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                identity.GetOrAdd(ClaimTypes.MobilePhone, user.PhoneNumber);
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                identity.GetOrAdd(ClaimTypes.Email, user.Email);
            }

            if (user is IMultiTenant multiTenant && multiTenant.TenantId.HasValue)
            {
                identity.GetOrAdd(TenantClaimTypes.TenantId, multiTenant.TenantId.ToString());
            }

            return principal;
        }
    }
}