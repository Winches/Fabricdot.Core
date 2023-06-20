using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

/// <summary>
///     UserRoleStore
/// </summary>
public partial class UserStore<TUser, TRole> : IUserRoleStore<TUser>//Parameter 'roleName' actually is normalized name.
{
    public virtual async Task AddToRoleAsync(
        TUser user,
        string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(normalizedRoleName, nameof(normalizedRoleName));

        var role = await RoleRepository.GetByNormalizedNameAsync(
            normalizedRoleName,
            false,
            cancellationToken) ?? throw new InvalidOperationException($"Role name '{normalizedRoleName}' does not exist.");
        await LoadCollectionAsync(user, v => v.Roles, cancellationToken);
        user.AddRole(role.Id);
    }

    public virtual async Task<IList<string>> GetRolesAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        // Aggregate root may not be found with query filter.
        //var userDetail = await UserRepository.GetDetailsByIdAsync(user.Id, cancellationToken);

        var roleNames = await UserRepository.ListRoleNamesAsync<TRole>(user.Id, cancellationToken);
        return roleNames.ToList();
    }

    public virtual async Task<IList<TUser>> GetUsersInRoleAsync(
        string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.NullOrEmpty(normalizedRoleName, nameof(normalizedRoleName));

        var role = await RoleRepository.GetByNormalizedNameAsync(
            normalizedRoleName,
            false,
            cancellationToken);
        if (role == null)
            return new List<TUser>();

        var users = await UserRepository.ListByRoleIdAsync(
            role.Id,
            cancellationToken: cancellationToken);
        return users.ToList();
    }

    public virtual async Task<bool> IsInRoleAsync(
        TUser user,
        string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(normalizedRoleName, nameof(normalizedRoleName));

        var role = await RoleRepository.GetByNormalizedNameAsync(
            normalizedRoleName,
            false,
            cancellationToken);
        if (role == null)
            return false;

        //role names
        var roles = await GetRolesAsync(user, cancellationToken);
        return roles.Any(v => LookupNormalizer.NormalizeName(v) == role.NormalizedName);
    }

    public virtual async Task RemoveFromRoleAsync(
        TUser user,
        string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));
        Guard.Against.NullOrEmpty(normalizedRoleName, nameof(normalizedRoleName));

        var role = await RoleRepository.GetByNormalizedNameAsync(
            normalizedRoleName,
            false,
            cancellationToken);
        if (role == null)
            return;

        await LoadCollectionAsync(user, v => v.Roles, cancellationToken);
        user.RemoveRole(role.Id);
    }
}
