using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserSecurityStampStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserSecurityStampStore<TUser>
{
    public virtual Task<string> GetSecurityStampAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.SecurityStamp);
    }

    public virtual Task SetSecurityStampAsync(
        TUser user,
        string stamp,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }
}
