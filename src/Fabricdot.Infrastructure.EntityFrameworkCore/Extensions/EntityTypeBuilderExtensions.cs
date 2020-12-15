using System;
using System.Linq;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.ValueObjects;
using Fabricdot.Infrastructure.Core.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Extensions
{
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
                    .HasMaxLength(AggregateRootBaseConstant.CONCURRENCY_STAMP_LEN)
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
                    .HasColumnName(nameof(IHasCreatorId.CreatorId));
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
                    .HasColumnName(nameof(IHasModifierId.LastModifierId));
            return builder;
        }

        #region soft-delete

        private static void ConfigureSoftDelete([NotNull] EntityTypeBuilder builder)
        {
            builder.Property(nameof(ISoftDelete.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName(nameof(ISoftDelete.IsDeleted));
        }

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
                    .HasColumnName(nameof(IHasDeleterId.DeleterId));
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

        #endregion

        /// <summary>
        ///     try configure GUID value
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        public static void TryConfigureGuidProperty(
            [NotNull] this EntityTypeBuilder builder,
            [NotNull] string propertyName)
        {
            var prop = builder.Property(propertyName);
            var clrType = prop.Metadata.ClrType;
            if (clrType.IsInstanceOfType(typeof(Guid)) || clrType.IsInstanceOfType(typeof(string)))
                prop.HasColumnType(DataConstant.ID_TYPE);
        }

        /// <summary>
        ///     try configure GUID value
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        public static void TryConfigureGuidProperty(
            [NotNull] this OwnedNavigationBuilder builder,
            [NotNull] string propertyName)
        {
            var prop = builder.Property(propertyName);
            var clrType = prop.Metadata.ClrType;
            if (clrType.IsInstanceOfType(typeof(Guid)) || clrType.IsInstanceOfType(typeof(string)))
                prop.HasColumnType(DataConstant.ID_TYPE);
        }

        /// <summary>
        ///     try configure implicitly primary key with GUID value
        /// </summary>
        /// <param name="builder"></param>
        public static void TryConfigureImplicitlyGuidPrimaryKey([NotNull] this EntityTypeBuilder builder)
        {
            if (!builder.Metadata.IsKeyless)
                return;
            builder.Property<Guid>(DataConstant.ID_NAME)
                .ValueGeneratedOnAdd()
                .HasValueGenerator<GuidValueGenerator>();
            builder.HasKey(DataConstant.ID_NAME);
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
            if (!typeof(Enumeration).IsAssignableFrom(prop.Metadata.ClrType))
                throw new ArgumentException($"{propertyName} is not an enumeration type.");

            var converter = new ValueConverter<TEnumeration, int>(v => v.Value,
                v => Enumeration.FromValue<TEnumeration>(v));
            return prop.HasConversion(converter);

        }

        /// <summary>
        ///     configure nested value object
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static EntityTypeBuilder ConfigureOwnedValueObject(
            [NotNull] this EntityTypeBuilder builder,
            [NotNull] string propertyName,
            [NotNull] Action<OwnedNavigationBuilder> buildAction)
        {
            var prop = builder.Metadata.GetNavigations().SingleOrDefault(v => v.Name == propertyName);
            if (prop == null)
                throw new ArgumentException($"{propertyName} is not defined.");

            builder.OwnsOne(prop.ClrType, propertyName, b =>
            {
                b.WithOwner();
                buildAction.Invoke(b);
            });

            return builder;
        }
    }
}