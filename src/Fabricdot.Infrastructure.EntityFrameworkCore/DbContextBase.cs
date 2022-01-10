﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.Domain.Auditing;
using Fabricdot.Infrastructure.Domain.Events;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public abstract class DbContextBase : DbContext
    {
        private IDataFilter _dataFilter;
        private IAuditPropertySetter _auditPropertySetter;
        private IDomainEventsDispatcher _domainEventsDispatcher;

        protected IDataFilter DataFilter => _dataFilter ??= this.GetRequiredService<IDataFilter>();
        protected IAuditPropertySetter AuditPropertySetter => _auditPropertySetter ??= this.GetRequiredService<IAuditPropertySetter>();
        protected IDomainEventsDispatcher DomainEventsDispatcher => _domainEventsDispatcher ??= this.GetRequiredService<IDomainEventsDispatcher>();

        protected bool HasSoftDeleteFilter => DataFilter?.IsEnabled<ISoftDelete>() ?? false;

        /// <inheritdoc />
        protected DbContextBase()
        {
            Initialize();
        }

        /// <inheritdoc />
        protected DbContextBase([NotNull] DbContextOptions options) : base(options)
        {
            Initialize();
        }

        public virtual async Task BeforeSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entityEntries = ChangeTracker.Entries().ToList();
            var changeInfos = EntityChangeInfoUtil.GetChangeInfos(entityEntries);
            await DomainEventsDispatcher.DispatchEventsAsync(changeInfos, cancellationToken);

            foreach (var entry in ChangeTracker.Entries())
                await HandleEntityEntryAsync(entry, cancellationToken);
        }

        public virtual Task AfterSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            await BeforeSaveChangesAsync(cancellationToken);
            var changes = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await AfterSaveChangesAsync(cancellationToken);
            return changes;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // `DbSet<T>` properties need be configured.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsOwned())
                    return;

                ApplyQueryFilter(modelBuilder, entityType);
            }
        }

        protected virtual void ApplyQueryFilter(
            ModelBuilder modelBuilder,
            IMutableEntityType mutableEntityType)
        {
            if (mutableEntityType.BaseType != null)
                return;

            var clrType = mutableEntityType.ClrType;
            LambdaExpression filter = null;

            var parameter = Expression.Parameter(mutableEntityType.ClrType);
            if (typeof(ISoftDelete).IsAssignableFrom(clrType))
            {
                Expression<Func<ISoftDelete, bool>> filterExpr = v => !HasSoftDeleteFilter || !v.IsDeleted;
                var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters[0], parameter, filterExpr.Body);
                filter = Expression.Lambda(body, parameter);
            }
            if (filter != null)
            {
                modelBuilder.Entity(clrType).HasQueryFilter(filter);
            }
        }

        protected virtual void Initialize()
        {
            ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.Immediate;
        }

        protected virtual void UpdateConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is not IHasConcurrencyStamp entity)
                return;
            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = NewConcurrencyStamp();
        }

        protected virtual void SetConcurrencyStamp(object entryEntity)
        {
            if (entryEntity is IHasConcurrencyStamp entity)
                entity.ConcurrencyStamp ??= NewConcurrencyStamp();
        }

        protected virtual async Task HandleEntityEntryAsync(EntityEntry entry, CancellationToken cancellationToken)
        {
            var entryEntity = entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    SetConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetCreationProperties(entryEntity);
                    AuditPropertySetter.SetModificationProperties(entryEntity);

                    break;

                case EntityState.Modified:
                    UpdateConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetModificationProperties(entryEntity);

                    break;

                case EntityState.Deleted:
                    if (!ShouldBeDeleteSoftly(entryEntity))
                        break;

                    await entry.ReloadAsync(cancellationToken);
                    UpdateConcurrencyStamp(entryEntity);
                    AuditPropertySetter.SetDeletionProperties(entryEntity);
                    entry.State = EntityState.Modified;

                    break;
            }
        }

        protected bool ShouldBeDeleteSoftly(object entity) => entity is ISoftDelete && DataFilter.IsEnabled<ISoftDelete>();

        private static string NewConcurrencyStamp() => Guid.NewGuid().ToString("N");
    }
}