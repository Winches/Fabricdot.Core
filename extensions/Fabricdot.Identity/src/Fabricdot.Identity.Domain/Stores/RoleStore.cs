using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Domain.Stores;

public partial class RoleStore<TRole> : IdentityStoreBase, IRoleStore<TRole> where TRole : IdentityRole
{
    protected IRoleRepository<TRole> RoleRepository { get; }

    public RoleStore(
        IRoleRepository<TRole> roleRepository,
        IGuidGenerator guidGenerator) : base(guidGenerator)
    {
        RoleRepository = roleRepository;
    }

    public virtual async Task<IdentityResult> CreateAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        await RoleRepository.AddAsync(role, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> UpdateAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        await RoleRepository.UpdateAsync(role, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<IdentityResult> DeleteAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        await RoleRepository.DeleteAsync(role, cancellationToken);
        await SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public virtual async Task<TRole> FindByIdAsync(
        string roleId,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await RoleRepository.GetDetailsByIdAsync(ConvertIdFromString(roleId), cancellationToken);
    }

    public virtual async Task<TRole> FindByNameAsync(
        string normalizedRoleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await RoleRepository.GetByNormalizedNameAsync(
            normalizedRoleName,
            cancellationToken: cancellationToken);
    }

    public virtual Task<string> GetNormalizedRoleNameAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        return Task.FromResult(role.NormalizedName);
    }

    public virtual Task<string?> GetRoleIdAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        return Task.FromResult(ConvertIdToString(role.Id));
    }

    public virtual Task<string> GetRoleNameAsync(
        TRole role,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        return Task.FromResult(role.Name);
    }

    public virtual Task SetNormalizedRoleNameAsync(
        TRole role,
        string normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public virtual Task SetRoleNameAsync(
        TRole role,
        string roleName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Guard.Against.Null(role, nameof(role));

        role.Name = roleName;
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
        TRole user,
        Expression<Func<TRole, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class
    {
        if (RoleRepository is not ISupportExplicitLoading<TRole> supportExplicitLoading)
            return;

        await supportExplicitLoading.LoadCollectionAsync(
            user,
            propertyExpression,
            cancellationToken);
    }

    protected virtual async Task LoadReferenceAsync<TProperty>(
        TRole user,
        Expression<Func<TRole, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default) where TProperty : class
    {
        if (RoleRepository is not ISupportExplicitLoading<TRole> supportExplicitLoading)
            return;

        await supportExplicitLoading.LoadReferenceAsync(
            user,
            propertyExpression,
            cancellationToken);
    }
}