using Ardalis.GuardClauses;
using Fabricdot.Authorization;
using Fabricdot.Authorization.Permissions;

namespace Microsoft.AspNetCore.Authorization;

public static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequirePermission(
        this AuthorizationPolicyBuilder builder,
        PermissionName permission)
    {
        Guard.Against.Null(builder, nameof(builder));

        builder.AddRequirements(new PermissionRequirement(permission));

        return builder;
    }

    public static AuthorizationPolicyBuilder RequirePermissions(
        this AuthorizationPolicyBuilder builder,
        ICollection<PermissionName> permissions,
        PermissionRequireBehavior requireBehavior)
    {
        Guard.Against.Null(builder, nameof(builder));

        builder.AddRequirements(new PermissionsRequirement(permissions, requireBehavior));

        return builder;
    }
}