using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Extensions
{
    public static class EfCoreConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyAccessMode"></param>
        public static IMutableNavigation SetNavigationProperty<T>(
            this EntityTypeBuilder<T> builder,
            string propertyName,
            PropertyAccessMode propertyAccessMode) where T : class
        {
            var navigation = builder.Metadata.FindNavigation(propertyName);
            navigation.SetPropertyAccessMode(propertyAccessMode);
            return navigation;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PropertyBuilder<TProperty> SetColumnType<T, TProperty>(
            this EntityTypeBuilder<T> builder,
            Expression<Func<T, TProperty>> propertyExpression,
            string typeName) where T : class
        {
            return builder
                .Property(propertyExpression)
                .HasColumnType(typeName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PropertyBuilder SetColumnType<T>(
            this EntityTypeBuilder<T> builder,
            string propertyName,
            string typeName) where T : class
        {
            return builder
                .Property(propertyName)
                .HasColumnType(typeName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRelated"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PropertyBuilder SetColumnType<T, TRelated>(
            this OwnedNavigationBuilder<T, TRelated> builder,
            string propertyName,
            string typeName) where T : class where TRelated : class
        {
            return builder
                .Property(propertyName)
                .HasColumnType(typeName);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRelated"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="builder"></param>
        /// <param name="propertyExpression"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static PropertyBuilder SetColumnType<T, TRelated, TProperty>(
            this OwnedNavigationBuilder<T, TRelated> builder,
            Expression<Func<TRelated, TProperty>> propertyExpression,
            string typeName) where T : class where TRelated : class
        {
            return builder
                .Property(propertyExpression)
                .HasColumnType(typeName);
        }
    }
}