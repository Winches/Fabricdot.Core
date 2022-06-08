using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserAuthenticationTokenStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserAuthenticationTokenStore<TUser>
{
    public virtual async Task<string?> GetTokenAsync(
        TUser user,
        string loginProvider,
        string name,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(loginProvider, nameof(loginProvider));
        Guard.Against.NullOrEmpty(name, nameof(name));

        await LoadCollectionAsync(user, v => v.Tokens, cancellationToken);
        var token = user.FindToken(loginProvider, name);
        return token?.Value;
    }

    public virtual async Task RemoveTokenAsync(
        TUser user,
        string loginProvider,
        string name,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(loginProvider, nameof(loginProvider));
        Guard.Against.NullOrEmpty(name, nameof(name));

        await LoadCollectionAsync(user, v => v.Tokens, cancellationToken);
        user.RemoveToken(loginProvider, name);
    }

    public virtual async Task SetTokenAsync(
        TUser user,
        string loginProvider,
        string name,
        string? value,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(loginProvider, nameof(loginProvider));
        Guard.Against.NullOrEmpty(name, nameof(name));

        await LoadCollectionAsync(user, v => v.Tokens, cancellationToken);
        user.AddOrUpdateToken(loginProvider, name, value);
    }
}