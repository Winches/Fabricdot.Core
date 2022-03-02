using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Infrastructure.Data
{
    public class IdentityDbContext<TUser, TRole> : DbContextBase, IIdentityDbContext<TUser, TRole> where TUser : IdentityUser where TRole : IdentityRole
    {
        public DbSet<TUser> Users { get; set; }

        public DbSet<IdentityUserClaim> UserClaims { get; set; }

        public DbSet<IdentityUserLogin> UserLogins { get; set; }

        public DbSet<IdentityUserToken> UserTokens { get; set; }

        public DbSet<IdentityUserRole> UserRoles { get; set; }

        public DbSet<TRole> Roles { get; set; }

        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }

        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureIdentity<TUser, TRole>();
        }
    }
}