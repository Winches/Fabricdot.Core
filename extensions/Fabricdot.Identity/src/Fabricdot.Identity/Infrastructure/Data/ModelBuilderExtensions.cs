using Fabricdot.Identity.Domain.Constants;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Infrastructure.Data;

public static class ModelBuilderExtensions
{
    private static string? _tablePerfix;
    private static string? _schema;

    public static void ConfigureIdentity<TUser, TRole>(
        this ModelBuilder modelBuilder,
        string tablePrefix = "Identity",
        string? schema = null) where TUser : IdentityUser where TRole : IdentityRole
    {
        _tablePerfix = tablePrefix;
        _schema = schema;
        ConfigureIdentityUser<TUser>(modelBuilder);
        ConfigureIdentityUserClaim(modelBuilder);
        ConfigureIdentityUserLogin(modelBuilder);
        ConfigureIdentityUserToken(modelBuilder);
        ConfigureIdentityUserRole<TRole>(modelBuilder);

        ConfigureIdentityRole<TRole>(modelBuilder);
        ConfigureIdentityRoleClaim(modelBuilder);
    }

    private static void ConfigureIdentityUser<TUser>(ModelBuilder modelBuilder) where TUser : IdentityUser
    {
        modelBuilder.Entity<TUser>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityUser, _schema);
            b.TryConfigure();

            b.Property(v => v.UserName)
             .IsRequired()
             .HasMaxLength(IdentityUserConstant.UserNameLength)
             .HasColumnName(nameof(IdentityUser.UserName));

            b.Property(v => v.NormalizedUserName)
             .IsRequired()
             .HasMaxLength(IdentityUserConstant.NormalizedNameLength)
             .HasColumnName(nameof(IdentityUser.NormalizedUserName));

            b.Property(v => v.Email)
             .HasMaxLength(IdentityUserConstant.EmailLength)
             .HasColumnName(nameof(IdentityUser.Email));

            b.Property(v => v.NormalizedEmail)
             .HasMaxLength(IdentityUserConstant.NormalizedEmailLength)
             .HasColumnName(nameof(IdentityUser.NormalizedEmail));

            b.Property(v => v.EmailConfirmed)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.EmailConfirmed));

            b.Property(v => v.GivenName)
             .HasMaxLength(IdentityUserConstant.GivenNameLength)
             .HasColumnName(nameof(IdentityUser.GivenName));

            b.Property(v => v.Surname)
             .HasMaxLength(IdentityUserConstant.SurnameLength)
             .HasColumnName(nameof(IdentityUser.Surname));

            b.Property(v => v.PasswordHash)
             .HasMaxLength(IdentityUserConstant.PasswordHashLength)
             .HasColumnName(nameof(IdentityUser.PasswordHash));

            b.Property(v => v.PhoneNumber)
             .HasMaxLength(IdentityUserConstant.PhoneNumberLength)
             .HasColumnName(nameof(IdentityUser.PhoneNumber));

            b.Property(v => v.PhoneNumberConfirmed)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.PhoneNumberConfirmed));

            b.Property(v => v.SecurityStamp)
             .IsRequired()
             .HasMaxLength(IdentityUserConstant.SecurityStampLength)
             .HasColumnName(nameof(IdentityUser.SecurityStamp));

            b.Property(v => v.IsActive)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.IsActive));

            b.Property(v => v.TwoFactorEnabled)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.TwoFactorEnabled));

            b.Property(v => v.LockoutEnd)
             .HasColumnName(nameof(IdentityUser.LockoutEnd));

            b.Property(v => v.LockoutEnabled)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.LockoutEnabled));

            b.Property(v => v.AccessFailedCount)
             .IsRequired()
             .HasColumnName(nameof(IdentityUser.AccessFailedCount));

            b.Navigation(v => v.Claims).UsePropertyAccessMode(PropertyAccessMode.Field);
            b.Navigation(v => v.Logins).UsePropertyAccessMode(PropertyAccessMode.Field);
            b.Navigation(v => v.Tokens).UsePropertyAccessMode(PropertyAccessMode.Field);
            b.Navigation(v => v.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);

            b.HasMany(v => v.Claims)
             .WithOne()
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            b.HasMany(v => v.Logins)
             .WithOne()
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            b.HasMany(v => v.Tokens)
             .WithOne()
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();
            b.HasMany(v => v.Roles)
             .WithOne()
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();

            b.HasIndex(v => v.UserName);
            b.HasIndex(v => v.NormalizedUserName);
            b.HasIndex(v => v.Email);
            b.HasIndex(v => v.NormalizedEmail);
        });
    }

    private static void ConfigureIdentityUserClaim(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserClaim>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityUserClaim, _schema);
            b.TryConfigure();

            b.Property(v => v.Id)
             .ValueGeneratedNever();

            b.Property(v => v.UserId)
             .IsRequired()
             .HasColumnName(nameof(IdentityUserClaim.UserId));

            b.Property(v => v.ClaimType)
             .IsRequired()
             .HasMaxLength(IdentityClaimConstant.TypeLength)
             .HasColumnName(nameof(IdentityUserClaim.ClaimType));

            b.Property(v => v.ClaimValue)
             .HasMaxLength(IdentityClaimConstant.ValueLength)
             .HasColumnName(nameof(IdentityUserClaim.ClaimValue));

            b.HasIndex(v => v.UserId);
        });
    }

    private static void ConfigureIdentityUserLogin(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserLogin>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityUserLogin, _schema);
            b.TryConfigure();

            b.HasKey(v => new { v.UserId, v.LoginProvider });

            b.Property(v => v.UserId)
             .IsRequired()
             .HasColumnName(nameof(IdentityUserLogin.UserId));

            b.Property(v => v.LoginProvider)
             .IsRequired()
             .HasMaxLength(IdentityUserLoginConstant.LoginProviderLength)
             .HasColumnName(nameof(IdentityUserLogin.LoginProvider));

            b.Property(v => v.ProviderKey)
             .IsRequired()
             .HasMaxLength(IdentityUserLoginConstant.ProviderKeyLength)
             .HasColumnName(nameof(IdentityUserLogin.ProviderKey));

            b.Property(v => v.ProviderDisplayName)
             .HasMaxLength(IdentityUserLoginConstant.ProviderDisplayNameLength)
             .HasColumnName(nameof(IdentityUserLogin.ProviderDisplayName));

            b.HasIndex(v => new { v.LoginProvider, v.ProviderKey });
        });
    }

    private static void ConfigureIdentityUserToken(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserToken>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityUserToken, _schema);
            b.TryConfigure();

            b.HasKey(v => new { v.UserId, v.LoginProvider, v.Name });

            b.Property(v => v.UserId)
             .IsRequired()
             .HasColumnName(nameof(IdentityUserToken.UserId));

            b.Property(v => v.LoginProvider)
             .IsRequired()
             .HasMaxLength(IdentityUserTokenConstant.LoginProviderLength)
             .HasColumnName(nameof(IdentityUserToken.LoginProvider));

            b.Property(v => v.Name)
             .IsRequired()
             .HasMaxLength(IdentityUserTokenConstant.NameLength)
             .HasColumnName(nameof(IdentityUserToken.Name));

            b.Property(v => v.Value)
             .HasColumnName(nameof(IdentityUserToken.Value));

            b.HasIndex(v => v.UserId);
        });
    }

    private static void ConfigureIdentityUserRole<TRole>(ModelBuilder modelBuilder) where TRole : IdentityRole
    {
        modelBuilder.Entity<IdentityUserRole>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityUserRole, _schema);
            b.TryConfigure();

            b.HasKey(v => new { v.UserId, v.RoleId });

            b.Property(v => v.UserId)
             .IsRequired()
             .HasColumnName(nameof(IdentityUserRole.UserId));

            b.Property(v => v.RoleId)
             .IsRequired()
             .HasColumnName(nameof(IdentityUserRole.RoleId));

            b.HasOne<TRole>()
             .WithMany()
             .HasForeignKey(v => v.RoleId);

            b.HasIndex(v => new { v.UserId, v.RoleId });
            b.HasIndex(v => v.RoleId);
        });
    }

    private static void ConfigureIdentityRole<TRole>(ModelBuilder modelBuilder) where TRole : IdentityRole
    {
        modelBuilder.Entity<TRole>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityRole, _schema);
            b.TryConfigure();

            b.Property(v => v.Name)
             .IsRequired()
             .HasMaxLength(IdentityRoleConstant.NameLength)
             .HasColumnName(nameof(IdentityRole.Name));

            b.Property(v => v.NormalizedName)
             .IsRequired()
             .HasMaxLength(IdentityRoleConstant.NormalizedNameLength)
             .HasColumnName(nameof(IdentityRole.NormalizedName));

            b.Navigation(v => v.Claims)
             .UsePropertyAccessMode(PropertyAccessMode.Field);

            b.HasMany(v => v.Claims)
             .WithOne()
             .HasForeignKey(v => v.RoleId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();

            b.HasIndex(v => v.NormalizedName);
        });
    }

    private static void ConfigureIdentityRoleClaim(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRoleClaim>(b =>
        {
            b.ToTable(_tablePerfix + IdentitySchema.IdentityRoleClaim, _schema);
            b.TryConfigure();

            b.Property(v => v.Id)
             .ValueGeneratedNever();

            b.Property(v => v.RoleId)
             .IsRequired()
             .HasColumnName(nameof(IdentityRoleClaim.RoleId));

            b.Property(v => v.ClaimType)
             .IsRequired()
             .HasMaxLength(IdentityClaimConstant.TypeLength)
             .HasColumnName(nameof(IdentityRoleClaim.ClaimType));

            b.Property(v => v.ClaimValue)
             .HasMaxLength(IdentityClaimConstant.ValueLength)
             .HasColumnName(nameof(IdentityRoleClaim.ClaimValue));

            b.HasIndex(v => v.RoleId);
        });
    }
}