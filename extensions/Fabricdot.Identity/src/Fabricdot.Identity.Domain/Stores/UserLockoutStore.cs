using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserLockoutStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserLockoutStore<TUser>
{
    public virtual Task<int> GetAccessFailedCountAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.AccessFailedCount);
    }

    public virtual Task<bool> GetLockoutEnabledAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.LockoutEnabled);
    }

    public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.LockoutEnd);
    }

    public virtual Task<int> IncrementAccessFailedCountAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.AccessFailed());
    }

    public virtual Task ResetAccessFailedCountAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.ResetAccessFailedCount();
        return Task.CompletedTask;
    }

    public virtual Task SetLockoutEnabledAsync(
        TUser user,
        bool enabled,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.LockoutEnabled = enabled;
        return Task.CompletedTask;
    }

    public virtual Task SetLockoutEndDateAsync(
        TUser user,
        DateTimeOffset? lockoutEnd,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        if (lockoutEnd.HasValue)
            user.Lockout(lockoutEnd.Value);
        else
            user.Unlock();
        return Task.CompletedTask;
    }
}