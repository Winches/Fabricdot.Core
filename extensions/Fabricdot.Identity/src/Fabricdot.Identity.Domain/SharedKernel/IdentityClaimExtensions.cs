using System.Security.Claims;
using Ardalis.GuardClauses;

namespace Fabricdot.Identity.Domain.SharedKernel
{
    public static class IdentityClaimExtensions
    {
        public static Claim ToClaim(this IIdentityClaim identityClaim)
        {
            Guard.Against.Null(identityClaim, nameof(identityClaim));
            return new Claim(identityClaim.ClaimType, identityClaim.ClaimValue);
        }
    }
}