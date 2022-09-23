using System.Collections.Immutable;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization.Permissions;

[Dependency(ServiceLifetime.Singleton)]
public class PermissionManager : IPermissionManager
{
    private readonly Lazy<Dictionary<PermissionName, Permission>> _permissions = new();
    private readonly Lazy<Dictionary<string, PermissionGroup>> _permissionGroups = new();
    protected IDictionary<string, PermissionGroup> PermissionGroups => _permissionGroups.Value;
    protected IDictionary<PermissionName, Permission> Permissions => _permissions.Value;

    public Task AddGroupAsync(PermissionGroup permissionGroup)
    {
        Guard.Against.Null(permissionGroup, nameof(permissionGroup));

        PermissionGroups.Add(permissionGroup.Name, permissionGroup);
        permissionGroup.Permissions.ForEach(v => AddPermissionRecursively(Permissions, v));
        return Task.CompletedTask;
    }

    public Task<Permission?> GetByNameAsync(PermissionName name)
    {
        var ret = Permissions.GetOrDefault(name);
        return Task.FromResult(ret);
    }

    public Task<IReadOnlyCollection<Permission>> ListAsync()
    {
        var ret = (IReadOnlyCollection<Permission>)Permissions.Values.ToImmutableList();
        return Task.FromResult(ret);
    }

    public Task<IReadOnlyCollection<PermissionGroup>> ListGroupsAsync()
    {
        var ret = (IReadOnlyCollection<PermissionGroup>)PermissionGroups.Values.ToImmutableList();
        return Task.FromResult(ret);
    }

    protected virtual void AddPermissionRecursively(
        IDictionary<PermissionName, Permission> pool,
        Permission permission)
    {
        if (pool.ContainsKey(permission.Name))
            throw new InvalidOperationException($"Permission '{permission.Name}' is already exists.");

        pool[permission.Name] = permission;

        foreach (var child in permission.Children)
        {
            AddPermissionRecursively(pool, child);
        }
    }
}