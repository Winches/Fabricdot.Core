using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserTwoFactorRecoveryCodeStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserTwoFactorRecoveryCodeStore<TUser>
{
    public virtual async Task<int> CountCodesAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        var source = await ListRecoveryCodeAsync(user, cancellationToken);
        return source.Length;
    }

    public virtual async Task<bool> RedeemCodeAsync(
        TUser user,
        string code,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.Null(code, nameof(code));

        var source = await ListRecoveryCodeAsync(user, cancellationToken);
        if (source.Contains(code))
        {
            var recoveryCodes = new List<string>(source.Where(s => s != code));
            await ReplaceCodesAsync(user, recoveryCodes, cancellationToken);
            return true;
        }

        return false;

        throw new NotImplementedException();
    }

    public virtual Task ReplaceCodesAsync(
        TUser user,
        IEnumerable<string> recoveryCodes,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        var value = string.Join(UserStoreConstants.RecoveryCodeTokenSeparator, recoveryCodes);
        return SetTokenAsync(
            user,
            UserStoreConstants.InternalLoginProvider,
            UserStoreConstants.RecoveryCodeTokenName,
            value,
            cancellationToken);
    }

    protected virtual async Task<string[]> ListRecoveryCodeAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        var code = await GetTokenAsync(
            user,
            UserStoreConstants.InternalLoginProvider,
            UserStoreConstants.RecoveryCodeTokenName,
            cancellationToken);

        return code == null ? Array.Empty<string>() : code.Split(UserStoreConstants.RecoveryCodeTokenSeparator);
    }
}