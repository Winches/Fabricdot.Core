using System.Security.Claims;
using Ardalis.GuardClauses;
using Fabricdot.Identity.Domain.SharedKernel;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserClaimStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserClaimStore<TUser>
{
    public virtual async Task AddClaimsAsync(
        TUser user,
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(claims, nameof(claims));

        await LoadCollectionAsync(user, v => v.Claims, cancellationToken);
        claims.ForEach(v => user.AddClaim(
            GuidGenerator.Create(),
            v.Type,
            v.Value));
    }

    public virtual async Task ReplaceClaimAsync(
        TUser user,
        Claim claim,
        Claim newClaim,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.Null(claim, nameof(claim));
        Guard.Against.Null(newClaim, nameof(newClaim));

        await LoadCollectionAsync(user, v => v.Claims, cancellationToken);
        user.ReplaceClaim(
            claim.Type,
            claim.Value,
            newClaim.Type,
            newClaim.Value);
    }

    public virtual async Task RemoveClaimsAsync(
        TUser user,
        IEnumerable<Claim> claims,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(claims, nameof(claims));

        await LoadCollectionAsync(user, v => v.Claims, cancellationToken);
        claims.ForEach(v => user.RemoveClaim(v.Type, v.Value));
    }

    public virtual async Task<IList<Claim>> GetClaimsAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        // TODO:Consider repository method.
        await LoadCollectionAsync(user, v => v.Claims, cancellationToken);
        return user.Claims.Select(v => v.ToClaim()).ToList();
    }

    public virtual async Task<IList<TUser>> GetUsersForClaimAsync(
        Claim claim,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(claim, nameof(claim));

        var users = await UserRepository.ListByClaimAsync(
            claim.Type,
            claim.Value,
            cancellationToken: cancellationToken);
        return users.ToList();
    }
}
