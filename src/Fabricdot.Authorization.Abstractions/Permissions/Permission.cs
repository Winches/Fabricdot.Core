using Ardalis.GuardClauses;

namespace Fabricdot.Authorization.Permissions;

public class Permission
{
    private readonly HashSet<Permission> _permissions = new();

    private string _displayName = null!;

    public PermissionName Name { get; private set; }

    public string DisplayName
    {
        get => _displayName;
        set
        {
            _displayName = Guard.Against.NullOrWhiteSpace(value, nameof(DisplayName));
        }
    }

    public string? Description { get; set; }

    public IReadOnlyCollection<Permission> Children => _permissions;

    public Permission(
        PermissionName name,
        string displayName,
        string? description = null,
        IEnumerable<Permission>? permissions = null)
    {
        Name = name;
        DisplayName = displayName;
        Description = description;

        if (!permissions.IsNullOrEmpty())
        {
            _permissions.UnionWith(permissions!);
        }
    }

    public Permission Add(
        PermissionName name,
        string displayName,
        string? description = null)
    {
        var permission = new Permission(
            name,
            displayName,
            description);
        _permissions.AddIfNotContains(permission);

        return this;
    }

    public Permission Remove(PermissionName name)
    {
        _permissions.RemoveAll(v => v.Name == name);
        return this;
    }

    public override bool Equals(object? obj) => obj is Permission permission && Name == permission.Name;

    public override int GetHashCode() => HashCode.Combine(Name);

    public override string ToString() => $"Permission:{Name}";
}
