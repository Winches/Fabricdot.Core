﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public abstract class EfRepository<TDbContext, T, TKey> : IRepository<T, TKey>
        where TDbContext : DbContext where T : class, IAggregateRoot, Domain.Core.Entities.IEntity<TKey>
    {
        protected readonly TDbContext Context;
        private readonly IDataFilter _filter;

        protected IQueryable<T> Entities => ApplyQueryFilter(Context.Set<T>());

        protected EfRepository(TDbContext context)
        {
            Context = context;
            _filter = context.GetRequiredService<IDataFilter>(); //singleton
        }

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await Context.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Remove(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task DeleteRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            Context.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            // Id of GUID type will become binary parameter when use MySql.Data driver
            // https://stackoverflow.com/questions/65503169/entity-framework-core-generate-wrong-guid-parameter-with-mysql
            return await Entities.SingleOrDefaultAsync(v => v.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<T> GetBySpecAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var entities = ApplySpecification(specification);
            return await entities.SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await Entities.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var entities = ApplySpecification(specification);
            return await entities.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            return await Entities.CountAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<int> CountAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var entities = ApplySpecification(specification);
            return await entities.CountAsync(cancellationToken);
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            var evaluator = new SpecificationEvaluator<T>();
            return evaluator.GetQuery(Entities, specification);
        }

        protected IQueryable<T> ApplyQueryFilter(IQueryable<T> entities)
        {
            if (_filter.IsEnabled<ISoftDelete>() && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
                // ReSharper disable once SuspiciousTypeConversion.Global
                return entities.Where(v => ((ISoftDelete)v).IsDeleted == false);

            return entities;
        }
    }
}