using System.Security.Claims;
using Fabricdot.Core.Security;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fabricdot.Identity;

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
            identity.GetOrAdd(SharedClaimTypes.GivenName, user.GivenName);
        }

        if (!string.IsNullOrWhiteSpace(user.Surname))
        {
            identity.GetOrAdd(SharedClaimTypes.Surname, user.Surname);
        }

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            identity.GetOrAdd(SharedClaimTypes.MobilePhone, user.PhoneNumber);
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            identity.GetOrAdd(SharedClaimTypes.Email, user.Email);
        }

        if (user is IMultiTenant multiTenant && multiTenant.TenantId.HasValue)
        {
            identity.GetOrAdd(TenantClaimTypes.TenantId, multiTenant.TenantId?.ToString() ?? string.Empty);
        }

        return principal;
    }
}