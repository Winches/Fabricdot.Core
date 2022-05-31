using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Fabricdot.PermissionGranting.Domain;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.PermissionGranting.Infrastructure.Data;

public static class ModelBuilderExtensions
{
    public static void ConfigurePermissionGranting(
        this ModelBuilder modelBuilder,
        string name = PermissionGrantingSchema.GrantedPermission,
        string schema = null)
    {
        modelBuilder.Entity<GrantedPermission>(b =>
        {
            b.ToTable(name, schema);
            b.TryConfigure();

            b.Property(v => v.GrantType)
             .HasColumnName(nameof(GrantedPermission.GrantType))
             .HasMaxLength(GrantPermissionConstant.GrantTypeLength)
             .IsRequired();

            b.Property(v => v.Subject)
             .HasColumnName(nameof(GrantedPermission.Subject))
             .HasMaxLength(GrantPermissionConstant.SubjectLength)
             .IsRequired();

            b.Property(v => v.Object)
             .HasColumnName(nameof(GrantedPermission.Object))
             .HasMaxLength(GrantPermissionConstant.ObjectLength)
             .IsRequired();

            b.HasIndex(v => new { v.TenantId, v.GrantType, v.Subject, v.Object })
             .IsUnique();
        });
    }
}