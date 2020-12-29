using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public abstract class EfRepository<T, TKey> : IRepository<T, TKey>
        where T : class, IAggregateRoot, Domain.Core.Entities.IEntity<TKey>
    {
        protected readonly DbContext Context;
        private readonly IDataFilter _filter;

        private DbSet<T> Set => Context.Set<T>();
        private IQueryable<T> Entities => ApplyQueryFilter(Set);

        protected EfRepository(DbContext context, IServiceProvider serviceProvider)
        {
            Context = context;
            _filter = serviceProvider.GetRequiredService<IDataFilter>();//singleton
        }

        /// <inheritdoc />
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Set.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            Set.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await Entities.SingleOrDefaultAsync(v => v.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T> GetBySpecAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var entities = ApplySpecification(specification);
            return await entities.SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await Entities.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var entities = ApplySpecification(specification);
            return await entities.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(
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