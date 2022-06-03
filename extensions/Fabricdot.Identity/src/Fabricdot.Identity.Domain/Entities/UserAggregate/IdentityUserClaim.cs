using System;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Fabricdot.Identity.Domain.SharedKernel;

namespace Fabricdot.Identity.Domain.Entities.UserAggregate
{
    public class IdentityUserClaim : Entity<Guid>, IIdentityClaim
    {
        public Guid UserId { get; private set; }

        public string ClaimType { get; private set; } = null!;

        public string? ClaimValue { get; private set; }

        public IdentityUserClaim(
            Guid userClaimId,
            string claimType,
            string? claimValue)
        {
            Id = userClaimId;
            SetClaim(claimType, claimValue);
        }

        private IdentityUserClaim()
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
}