using System;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.Identity.Tests.Entities
{
    public class Role : IdentityRole, IMultiTenant
    {
        public Guid? TenantId { get; private set; }

        public Role(
            Guid roleId,
            string roleName,
            Guid? tenantId = null) : base(roleId, roleName)
        {
            TenantId = tenantId;
        }

        private Role()
        {
        }
    }
}