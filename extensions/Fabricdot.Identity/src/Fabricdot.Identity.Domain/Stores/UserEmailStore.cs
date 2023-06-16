using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserEmailStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserEmailStore<TUser>
{
    public virtual async Task<TUser> FindByEmailAsync(
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.NullOrEmpty(normalizedEmail, nameof(normalizedEmail));

        return (await UserRepository.GetByNormalizedEmailAsync(
            normalizedEmail,
            false,
            cancellationToken))!;
    }

    public virtual Task<string?> GetEmailAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.Email);
    }

    public virtual Task<bool> GetEmailConfirmedAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.EmailConfirmed);
    }

    public virtual Task<string?> GetNormalizedEmailAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.NormalizedEmail);
    }

    public virtual Task SetEmailAsync(
        TUser user,
        string email,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.Email = email;
        return Task.CompletedTask;
    }

    public virtual Task SetEmailConfirmedAsync(
        TUser user,
        bool confirmed,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public virtual Task SetNormalizedEmailAsync(
        TUser user,
        string normalizedEmail,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }
}
