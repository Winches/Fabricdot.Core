using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserPhoneNumberStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserPhoneNumberStore<TUser>
{
    public virtual Task<string?> GetPhoneNumberAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.PhoneNumber);
    }

    public virtual Task<bool> GetPhoneNumberConfirmedAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.PhoneNumberConfirmed);
    }

    public virtual Task SetPhoneNumberAsync(
        TUser user,
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.ChangePhoneNumber(phoneNumber);
        return Task.CompletedTask;
    }

    public virtual Task SetPhoneNumberConfirmedAsync(
        TUser user,
        bool confirmed,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.ChangePhoneNumber(user.PhoneNumber, confirmed);
        return Task.CompletedTask;
    }
}