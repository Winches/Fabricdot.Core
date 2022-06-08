using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserTwoFactorStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserTwoFactorStore<TUser>
{
    public virtual Task<bool> GetTwoFactorEnabledAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.TwoFactorEnabled);
    }

    public virtual Task SetTwoFactorEnabledAsync(
        TUser user,
        bool enabled,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.TwoFactorEnabled = enabled;
        return Task.CompletedTask;
    }
}