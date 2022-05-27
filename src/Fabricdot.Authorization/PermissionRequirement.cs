using Fabricdot.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Fabricdot.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionName Permission { get; }

        public PermissionRequirement(PermissionName permission)
        {
            Permission = permission;
        }
    }
}