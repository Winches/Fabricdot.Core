using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Fabricdot.Authorization
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        public ICollection<PermissionName> Permissions { get; }

        public PermissionRequireBehavior RequireBehavior { get; }

        public PermissionsRequirement(
            ICollection<PermissionName> names,
            PermissionRequireBehavior requireBehavior)
        {
            Permissions = Guard.Against.Null(names, nameof(names));
            RequireBehavior = Guard.Against.EnumOutOfRange(requireBehavior, nameof(requireBehavior));
        }
    }
}