using System.Security.Claims;

namespace Fabricdot.Identity;

public static class IdentityClaimTypes
{
    public const string UserId = ClaimTypes.NameIdentifier;

    public const string RoleId = "claims/roleid";
}
