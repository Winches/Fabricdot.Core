namespace Fabricdot.Authorization.Permissions;

public interface IPermissionManager
{
    Task AddGroupAsync(PermissionGroup permissionGroup);

    Task<Permission?> GetByNameAsync(PermissionName name);

    Task<IReadOnlyCollection<Permission>> ListAsync();

    Task<IReadOnlyCollection<PermissionGroup>> ListGroupsAsync();
}
