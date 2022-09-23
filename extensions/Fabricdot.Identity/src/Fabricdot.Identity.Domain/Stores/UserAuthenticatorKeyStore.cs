using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserAuthenticatorKeyStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserAuthenticatorKeyStore<TUser>
{
    public virtual Task<string?> GetAuthenticatorKeyAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        return GetTokenAsync(
            user,
            UserStoreConstants.InternalLoginProvider,
            UserStoreConstants.AuthenticatorKeyTokenName,
            cancellationToken);
    }

    public virtual Task SetAuthenticatorKeyAsync(
        TUser user,
        string key,
        CancellationToken cancellationToken)
    {
        return SetTokenAsync(
            user,
            UserStoreConstants.InternalLoginProvider,
            UserStoreConstants.AuthenticatorKeyTokenName,
            key,
            cancellationToken);
    }
}