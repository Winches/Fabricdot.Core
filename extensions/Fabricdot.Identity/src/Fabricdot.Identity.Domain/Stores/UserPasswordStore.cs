using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserPasswordStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserPasswordStore<TUser>
{
    public virtual Task<string?> GetPasswordHashAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.PasswordHash);
    }

    public virtual Task<bool> HasPasswordAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.HasPassword);
    }

    public virtual Task SetPasswordHashAsync(
        TUser user,
        string passwordHash,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }
}
