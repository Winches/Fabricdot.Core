using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Domain.ValueObjects;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    ///     get entity type
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static Type GetClrType(this EntityTypeBuilder builder)
    {
        return builder.Metadata.ClrType;
    }

    /// <summary>
    ///     try configure entity by convention
    /// </summary>
    /// <param name="builder"></param>
    public static void TryConfigure(this EntityTypeBuilder builder)
    {
        builder.TryConfigureTypedKey();

        builder.TryConfigureConcurrencyStamp();

        builder.TryConfigureCreationTime();
        builder.TryConfigureCreatorId();

        builder.TryConfigureModificationTime();
        builder.TryConfigureModifierId();

        builder.TryConfigureSoftDelete();
        builder.TryConfigureDeletionTime();
        builder.TryConfigureDeleterId();

        builder.TryConfigureMultiTenant();
    }

    /// <summary>
    ///     Try configure typed key which is derived from <see cref="IIdentity{T}" />
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static EntityTypeBuilder TryConfigureTypedKey(this EntityTypeBuilder builder)
    {
        var type = builder.GetClrType();
        if (type.IsAssignableToGenericType(typeof(IEntity<>)))
        {
            const string name = nameof(IEntity<object>.Id);
            var propertyBuilder = builder.Property(name);
            if (propertyBuilder.Metadata.ClrType.IsAssignableToGenericType(typeof(IIdentity<>)))
            {
                builder.HasKey(name);
                propertyBuilder.IsTypedKey();
            }
        }
        return builder;
    }

    /// <summary>
    ///     try configure concurrency token
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureConcurrencyStamp(this EntityTypeBuilder builder)
    {
        if (typeof(IHasConcurrencyStamp).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .IsRequired()
                .HasMaxLength(AggregateRootConstants.ConcurrencyStampLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        }

        return builder;
    }

    /// <summary>
    ///     try configure creation time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureCreationTime(this EntityTypeBuilder builder)
    {
        if (typeof(IHasCreationTime).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasCreationTime.CreationTime))
                .IsRequired()
                .HasColumnName(nameof(IHasCreationTime.CreationTime));
        }

        return builder;
    }

    /// <summary>
    ///     try configure creator id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureCreatorId(this EntityTypeBuilder builder)
    {
        if (typeof(IHasCreatorId).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasCreatorId.CreatorId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasCreatorId.CreatorId))
                .HasMaxLength(AuditConstant.UserIdLength);
        }

        return builder;
    }

    /// <summary>
    ///     try configure modification time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureModificationTime(this EntityTypeBuilder builder)
    {
        if (typeof(IHasModificationTime).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasModificationTime.LastModificationTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModificationTime.LastModificationTime));
        }

        return builder;
    }

    /// <summary>
    ///     try configure modifier id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureModifierId(this EntityTypeBuilder builder)
    {
        if (typeof(IHasModifierId).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasModifierId.LastModifierId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModifierId.LastModifierId))
                .HasMaxLength(AuditConstant.UserIdLength);
        }

        return builder;
    }

    #region soft-delete

    /// <summary>
    ///     try configure soft-delete
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureSoftDelete(this EntityTypeBuilder builder)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(builder.GetClrType()))
            ConfigureSoftDelete(builder);

        return builder;
    }

    /// <summary>
    ///     try configure deleter id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureDeleterId(this EntityTypeBuilder builder)
    {
        if (typeof(IHasDeleterId).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasDeleterId.DeleterId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeleterId.DeleterId))
                .HasMaxLength(AuditConstant.UserIdLength);

            ConfigureSoftDelete(builder);
        }

        return builder;
    }

    /// <summary>
    ///     try configure deletion time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureDeletionTime(this EntityTypeBuilder builder)
    {
        if (typeof(IHasDeletionTime).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasDeletionTime.DeletionTime))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeletionTime.DeletionTime));

            ConfigureSoftDelete(builder);
        }

        return builder;
    }

    private static void ConfigureSoftDelete(EntityTypeBuilder builder)
    {
        builder.Property(nameof(ISoftDelete.IsDeleted))
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(ISoftDelete.IsDeleted));
    }

    #endregion soft-delete

    public static void TryConfigureMultiTenant(this EntityTypeBuilder builder)
    {
        if (!typeof(IMultiTenant).IsAssignableFrom(builder.GetClrType()))
            return;

        builder.Property(nameof(IMultiTenant.TenantId))
               .IsRequired(false)
               .HasColumnName(nameof(IMultiTenant.TenantId));
    }

    /// <summary>
    ///     try configure navigation property
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <param name="propertyAccessMode"></param>
    [Obsolete("Use property builder")]
    public static void TryConfigureNavigationProperty(
        this EntityTypeBuilder builder,
        string propertyName,
        PropertyAccessMode propertyAccessMode)
    {
        var navigation = builder.Metadata.FindNavigation(propertyName);
        navigation?.SetPropertyAccessMode(propertyAccessMode);
    }

    /// <summary>
    ///     configure enumeration type property
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <exception cref="ArgumentException"></exception>
    [Obsolete("Use 'IsEnumeration' method from property builder")]
    public static PropertyBuilder ConfigureEnumeration<TEnumeration>(
        this EntityTypeBuilder builder,
        string propertyName)
        where TEnumeration : Enumeration
    {
        return builder.Property(propertyName).IsEnumeration();
        //return ConfigureEnumeration<TEnumeration>(prop);
    }

    /// <summary>
    ///     configure enumeration type property
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <exception cref="ArgumentException"></exception>
    [Obsolete("Use 'IsEnumeration' method from property builder")]
    public static PropertyBuilder ConfigureEnumeration<TEnumeration>(
        this OwnedNavigationBuilder builder,
        string propertyName)
        where TEnumeration : Enumeration
    {
        return builder.Property(propertyName).IsEnumeration();
        //return ConfigureEnumeration<TEnumeration>(prop);
    }
}
