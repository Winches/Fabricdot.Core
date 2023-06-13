using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

// Reference: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-5.0

/// <summary>
///     UserStore
/// </summary>
public partial class UserStore<TUser, TRole> : IdentityStoreBase, IUserStore<TUser> where TUser : IdentityUser where TRole : IdentityRole
{
    protected IUserRepository<TUser> UserRepository { get; }
    protected IRoleRepository<TRole> RoleRepository { get; }
    protected ILookupNormalizer LookupNormalizer { get; }

    public UserStore(
        IUserRepository<TUser> userRepository,
        IRoleRepository<TRole> roleRepository,
        IGuidGenerator guidGenerator,
        ILookupNormalizer lookupNormalizer) : base(guidGenerator)
    {
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        LookupNormalizer = lookupNormalizer;
    }

    public virtual async Task<IdentityResult> CreateAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        await UserRepository.AddAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> UpdateAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        await UserRepository.UpdateAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> DeleteAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        await UserRepository.DeleteAsync(user, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<TUser> FindByIdAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return (await UserRepository.GetByIdAsync(
            ConvertIdFromString(userId),
            cancellationToken: cancellationToken))!;
    }

    public virtual async Task<TUser> FindByNameAsync(
        string normalizedUserName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return (await UserRepository.GetByNormalizedUserNameAsync(
            normalizedUserName,
            cancellationToken: cancellationToken))!;
    }

    public virtual Task<string> GetNormalizedUserNameAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.NormalizedUserName);
    }

    public virtual Task<string?> GetUserIdAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(ConvertIdToString(user.Id));
    }

    public virtual Task<string> GetUserNameAsync(
        TUser user,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        return Task.FromResult(user.UserName);
    }

    public virtual Task SetNormalizedUserNameAsync(
        TUser user,
        string normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public virtual Task SetUserNameAsync(
        TUser user,
        string userName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(user, nameof(user));

        user.UserName = userName;
        return Task.CompletedTask;
    }

    protected override Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (!AutoSaveChanges)
        {
            return Task.CompletedTask;
        }

        // TODO:Consider implement auto-save-changes
        throw new NotSupportedException();
    }

    protected virtual async Task LoadCollectionAsync<TProperty>(
        TUser user,
        Expression<Func<TUser, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class
    {
        if (UserRepository is not ISupportExplicitLoading<TUser> supportExplicitLoading)
            return;

        await supportExplicitLoading.LoadCollectionAsync(
            user,
            propertyExpression,
            cancellationToken);
    }

    protected virtual async Task LoadReferenceAsync<TProperty>(
        TUser user,
        Expression<Func<TUser, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class
    {
        if (UserRepository is not ISupportExplicitLoading<TUser> supportExplicitLoading)
            return;

        await supportExplicitLoading.LoadReferenceAsync(
            user,
            propertyExpression,
            cancellationToken);
    }
}