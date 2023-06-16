using Fabricdot.Domain.SharedKernel;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Tests.Entities;

public class Role : IdentityRole, IMultiTenant
{
    public Guid? TenantId { get; private set; }

    public Role(
    Guid roleId,
    string roleName) : base(roleId, roleName)
    {
    }

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
