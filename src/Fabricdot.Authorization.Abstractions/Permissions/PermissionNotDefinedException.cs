using System;
using System.Diagnostics.CodeAnalysis;

namespace Fabricdot.Authorization.Permissions
{
    [SuppressMessage("Roslynator", "RCS1194:Implement exception constructors.", Justification = "<Pending>")]
    public class PermissionNotDefinedException : Exception
    {
        public PermissionNotDefinedException(PermissionName permission) : base($"Permission:{permission} is not defined.")
        {
        }
    }
}