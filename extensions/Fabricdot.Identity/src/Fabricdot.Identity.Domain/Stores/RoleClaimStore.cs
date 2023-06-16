using System.Security.Claims;
using Ardalis.GuardClauses;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     RoleClaimStore
/// </summary>
public partial class RoleStore<TRole> : IRoleClaimStore<TRole> where TRole : IdentityRole
{
    public virtual async Task<IList<Claim>> GetClaimsAsync(
        TRole role,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        //var identityRole = await RoleRepository.GetByNormalizedNameAsync(
        //    role.NormalizedName,
        //    cancellationToken: cancellationToken);
        await LoadCollectionAsync(role, v => v.Claims, cancellationToken);
        return role.Claims.Select(v => new Claim(v.ClaimType, v.ClaimValue ?? string.Empty)).ToList();
    }

    public virtual async Task AddClaimAsync(
        TRole role,
        Claim claim,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));
        Guard.Against.Null(claim, nameof(claim));

        await LoadCollectionAsync(role, v => v.Claims, cancellationToken);
        role.AddClaim(GuidGenerator.Create(), claim.Type, claim.Value);
    }

    public virtual async Task RemoveClaimAsync(
        TRole role,
        Claim claim,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));
        Guard.Against.Null(claim, nameof(claim));

        await LoadCollectionAsync(role, v => v.Claims, cancellationToken);
        role.RemoveClaim(claim.Type, claim.Value);
    }
}
