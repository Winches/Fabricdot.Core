using System;
using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.SharedKernel;
using Fabricdot.Domain.ValueObjects;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    ///     get entity type
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static Type GetClrType([NotNull] this EntityTypeBuilder builder)
    {
        return builder.Metadata.ClrType;
    }

    /// <summary>
    ///     try configure entity by convention
    /// </summary>
    /// <param name="builder"></param>
    public static void TryConfigure([NotNull] this EntityTypeBuilder builder)
    {
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
    ///     try configure concurrency token
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureConcurrencyStamp([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasConcurrencyStamp).IsAssignableFrom(builder.GetClrType()))
            builder.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                .IsConcurrencyToken()
                .IsRequired()
                .HasMaxLength(AggregateRootConstants.ConcurrencyStampLength)
                .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
        return builder;
    }

    /// <summary>
    ///     try configure creation time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureCreationTime([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasCreationTime).IsAssignableFrom(builder.GetClrType()))
            builder.Property(nameof(IHasCreationTime.CreationTime))
                .IsRequired()
                .HasColumnName(nameof(IHasCreationTime.CreationTime));
        return builder;
    }

    /// <summary>
    ///     try configure creator id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureCreatorId([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasCreatorId).IsAssignableFrom(builder.GetClrType()))
            builder.Property(nameof(IHasCreatorId.CreatorId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasCreatorId.CreatorId))
                .HasMaxLength(AuditConstant.USER_ID_LEN);
        return builder;
    }

    /// <summary>
    ///     try configure modification time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureModificationTime([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasModificationTime).IsAssignableFrom(builder.GetClrType()))
            builder.Property(nameof(IHasModificationTime.LastModificationTime))
                .IsRequired()
                .HasColumnName(nameof(IHasModificationTime.LastModificationTime));
        return builder;
    }

    /// <summary>
    ///     try configure modifier id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureModifierId([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasModifierId).IsAssignableFrom(builder.GetClrType()))
            builder.Property(nameof(IHasModifierId.LastModifierId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasModifierId.LastModifierId))
                .HasMaxLength(AuditConstant.USER_ID_LEN);
        return builder;
    }

    #region soft-delete

    /// <summary>
    ///     try configure soft-delete
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureSoftDelete([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(builder.GetClrType()))
            ConfigureSoftDelete(builder);

        return builder;
    }

    /// <summary>
    ///     try configure deleter id
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureDeleterId([NotNull] this EntityTypeBuilder builder)
    {
        if (typeof(IHasDeleterId).IsAssignableFrom(builder.GetClrType()))
        {
            builder.Property(nameof(IHasDeleterId.DeleterId))
                .IsRequired(false)
                .HasColumnName(nameof(IHasDeleterId.DeleterId))
                .HasMaxLength(AuditConstant.USER_ID_LEN);
            //todo:consider use strong type user id,maybe GUID.

            ConfigureSoftDelete(builder);
        }

        return builder;
    }

    /// <summary>
    ///     try configure deletion time
    /// </summary>
    /// <param name="builder"></param>
    public static EntityTypeBuilder TryConfigureDeletionTime([NotNull] this EntityTypeBuilder builder)
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

    private static void ConfigureSoftDelete([NotNull] EntityTypeBuilder builder)
    {
        builder.Property(nameof(ISoftDelete.IsDeleted))
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(ISoftDelete.IsDeleted));
    }

    #endregion soft-delete

    public static void TryConfigureMultiTenant([NotNull] this EntityTypeBuilder builder)
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
    public static void TryConfigureNavigationProperty(
        [NotNull] this EntityTypeBuilder builder,
        [NotNull] string propertyName,
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
    public static PropertyBuilder ConfigureEnumeration<TEnumeration>(
        [NotNull] this EntityTypeBuilder builder,
        [NotNull] string propertyName)
        where TEnumeration : Enumeration
    {
        var prop = builder.Property(propertyName);
        return ConfigureEnumeration<TEnumeration>(prop);
    }

    /// <summary>
    ///     configure enumeration type property
    /// </summary>
    /// <typeparam name="TEnumeration"></typeparam>
    /// <param name="builder"></param>
    /// <param name="propertyName"></param>
    /// <exception cref="ArgumentException"></exception>
    public static PropertyBuilder ConfigureEnumeration<TEnumeration>(
        [NotNull] this OwnedNavigationBuilder builder,
        [NotNull] string propertyName)
        where TEnumeration : Enumeration
    {
        var prop = builder.Property(propertyName);
        return ConfigureEnumeration<TEnumeration>(prop);
    }

    private static PropertyBuilder ConfigureEnumeration<TEnumeration>(PropertyBuilder prop) where TEnumeration : Enumeration
    {
        var clrType = prop.Metadata.ClrType;
        if (!typeof(Enumeration).IsAssignableFrom(clrType))
            throw new ArgumentException($"{clrType.Name} is not an enumeration type.");

        var converter = new ValueConverter<TEnumeration, int>(v => v.Value,
            v => Enumeration.FromValue<TEnumeration>(v));
        return prop.HasConversion(converter);
    }
}