using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Fabricdot.Identity.Domain.SharedKernel;

namespace Fabricdot.Identity.Domain.Entities.RoleAggregate;

public class IdentityRoleClaim : Entity<Guid>, IIdentityClaim
{
    public Guid RoleId { get; private set; }

    public string ClaimType { get; private set; } = null!;

    public string? ClaimValue { get; private set; }

    public IdentityRoleClaim(
        Guid roleClaimId,
        string claimType,
        string? claimValue)
    {
        Id = roleClaimId;
        ClaimType = Guard.Against.NullOrEmpty(claimType, nameof(claimType));
        ClaimValue = claimValue;
    }

    private IdentityRoleClaim()
    {
    }

    public void SetClaim(
        string claimType,
        string? claimValue)
    {
        ClaimType = Guard.Against.NullOrEmpty(claimType, nameof(claimType));
        ClaimValue = claimValue;
    }
}
