using System.Security.Claims;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Security;

[Dependency(ServiceLifetime.Transient)]
public class CurrentUser : ICurrentUser
{
    private static readonly Claim[] s_emptyClaimsArray = Array.Empty<Claim>();

    private readonly IPrincipalAccessor _principalAccessor;

    public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

    public virtual string? Id => _principalAccessor.Principal?.Claims
        .FirstOrDefault(c => c.Type == SharedClaimTypes.NameIdentifier)
        ?.Value;

    public virtual string? UserName => FindClaim(SharedClaimTypes.Name)?.Value;

    public virtual string[] Roles => FindClaims(SharedClaimTypes.Role).Select(c => c.Value).ToArray();

    public CurrentUser(IPrincipalAccessor principalAccessor)
    {
        _principalAccessor = principalAccessor;
    }

    public virtual Claim? FindClaim(string claimType)
    {
        return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public virtual Claim[] FindClaims(string claimType)
    {
        return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? s_emptyClaimsArray;
    }

    public virtual Claim[] GetAllClaims()
    {
        return _principalAccessor.Principal?.Claims.ToArray() ?? s_emptyClaimsArray;
    }

    public virtual bool IsInRole(string roleName)
    {
        return FindClaims(SharedClaimTypes.Role).Any(c => c.Value == roleName);
    }
}
