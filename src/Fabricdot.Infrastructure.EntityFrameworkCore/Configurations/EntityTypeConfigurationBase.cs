﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Configurations
{
    public abstract class EntityTypeConfigurationBase<T> : IEntityTypeConfiguration<T> where T : class
    {
        private static readonly Type EntityType = typeof(T);
        protected virtual string IdName => DataConstant.ID_NAME;
        protected virtual string IdType => DataConstant.ID_TYPE;
        protected virtual IEnumerable<string> IgnoreAssociationProperty => Enumerable.Empty<string>();

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable($"{EntityType.Name}s");
            ConfigureAssociationProperty(builder);
        }

        protected void ConfigureOwnedNavigation<TRelated>(OwnedNavigationBuilder<T, TRelated> builder)
            where TRelated : class
        {
            var type = typeof(TRelated);
            var lowerIdName = IdName.ToLower();
            type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(v => v.Name.ToLower().EndsWith(lowerIdName))
                .ToList()
                .ForEach(v => { builder.SetColumnType(v.Name, IdType); });
        }

        private void ConfigureAssociationProperty(EntityTypeBuilder<T> builder)
        {
            var lowerIdName = IdName.ToLower();
            EntityType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(v => !IgnoreAssociationProperty.Contains(v.Name))
                .Where(v => v.Name.ToLower().EndsWith(lowerIdName))
                .ToList()
                .ForEach(v => { builder.SetColumnType(v.Name, IdType); });
        }
    }
}