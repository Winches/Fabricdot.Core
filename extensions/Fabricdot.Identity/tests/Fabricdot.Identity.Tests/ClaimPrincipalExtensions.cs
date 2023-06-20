using System.Security.Claims;

namespace Fabricdot.Identity.Tests;

public static class ClaimPrincipalExtensions
{
    public static ClaimPrincipalAssertions Should(this ClaimsPrincipal claimsPrincipal) => new(claimsPrincipal);
}
