using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserLoginStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserLoginStore<TUser>
{
    public virtual async Task AddLoginAsync(
        TUser user,
        UserLoginInfo login,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.Null(login, nameof(login));

        await LoadCollectionAsync(user, v => v.Logins, cancellationToken);
        user.AddLogin(
            login.LoginProvider,
            login.ProviderKey,
            login.ProviderDisplayName);
    }

    public virtual async Task RemoveLoginAsync(
        TUser user,
        string loginProvider,
        string providerKey,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        await LoadCollectionAsync(user, v => v.Logins, cancellationToken);
        user.RemoveLogin(loginProvider, providerKey);
    }

    public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        await LoadCollectionAsync(user, v => v.Logins, cancellationToken);
        return user.Logins.Select(v => new UserLoginInfo(v.LoginProvider, v.ProviderKey, v.ProviderDisplayName))
                         .ToList();
    }

    public virtual async Task<TUser> FindByLoginAsync(
        string loginProvider,
        string providerKey,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await UserRepository.GetByLoginAsync(
            loginProvider,
            providerKey,
            true,
            cancellationToken);
    }
}