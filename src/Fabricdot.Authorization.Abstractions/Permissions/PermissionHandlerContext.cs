using Ardalis.GuardClauses;

namespace Fabricdot.Authorization.Permissions;

public class PermissionHandlerContext
{
    private readonly HashSet<PermissionName> _pendingPermissions = new();

    public IReadOnlyCollection<GrantSubject> Subjects { get; }

    public IReadOnlyCollection<PermissionName> Permissions { get; }

    public IReadOnlyCollection<PermissionName> PendingPermissions => _pendingPermissions;

    public PermissionHandlerContext(
        IEnumerable<GrantSubject> subjects,
        IEnumerable<PermissionName> permissions)
    {
        Guard.Against.Null(subjects, nameof(subjects));
        Guard.Against.Null(permissions, nameof(permissions));

        Subjects = subjects.ToList();
        Permissions = permissions.ToList();
        _pendingPermissions.UnionWith(permissions);
    }

    public void Succeed(string permission) => _pendingPermissions.Remove(permission);

    public void Succeed() => _pendingPermissions.Clear();

    public IReadOnlySet<GrantResult> GetResults()
    {
        return Permissions.Select(v => new GrantResult(v.Value, !PendingPermissions.Contains(v))).ToHashSet();
    }
}