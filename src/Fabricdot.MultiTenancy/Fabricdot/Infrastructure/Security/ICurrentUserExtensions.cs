using System;
using Ardalis.GuardClauses;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.Infrastructure.Security
{
    public static class ICurrentUserExtensions
    {
        public static Guid? GetTenantId(this ICurrentUser currentUser)
        {
            Guard.Against.Null(currentUser, nameof(currentUser));

            var claim = currentUser.FindClaim(TenantClaimTypes.TenantId);
            return Guid.TryParse(claim?.Value, out var tenantId) ? tenantId : null;
        }
    }
}