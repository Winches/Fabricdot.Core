using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Infrastructure.Data;

public interface IIdentityDbContext<TUser, TRole>
    where TUser : IdentityUser
    where TRole : IdentityRole
{
    DbSet<TUser> Users { get; }

    DbSet<IdentityUserClaim> UserClaims { get; }

    DbSet<IdentityUserLogin> UserLogins { get; }

    DbSet<IdentityUserRole> UserRoles { get; }

    DbSet<IdentityUserToken> UserTokens { get; }

    DbSet<TRole> Roles { get; }

    DbSet<IdentityRoleClaim> RoleClaims { get; }
}