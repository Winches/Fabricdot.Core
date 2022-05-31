using Fabricdot.PermissionGranting.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.PermissionGranting.Infrastructure.Data;

public interface IPermissionGrantingDbContext
{
    DbSet<GrantedPermission> GrantedPermissions { get; set; }
}
