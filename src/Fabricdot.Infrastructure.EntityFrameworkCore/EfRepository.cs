using System;
using System.Collections.Generic;
using System.Linq;
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

        protected EfRepository(DbContext context, IServiceProvider serviceProvider)
        {
            Context = context;
            _filter = serviceProvider.GetRequiredService<IDataFilter>();
        }

        /// <inheritdoc />
        public async Task<T> AddAsync(T entity)
        {
            await Set.AddAsync(entity);
            return entity;
        }

        /// <inheritdoc />
        public Task DeleteAsync(T entity)
        {
            Set.Remove(entity);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            Set.RemoveRange(entities);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<T> GetByIdAsync(TKey id)
        {
            var entity = await Set.FindAsync(id);
            return ApplyQueryFilter(entity);
        }

        /// <inheritdoc />
        public async Task<T> GetBySpecAsync(ISpecification<T> specification)
        {
            var res = ApplySpecification(specification);
            var entity = await res.SingleOrDefaultAsync();
            return ApplyQueryFilter(entity);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            var entities = ApplyQueryFilter(Set);
            return await entities.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> ListAllAsync(ISpecification<T> specification)
        {
            var res = ApplySpecification(specification);
            return await res.ToListAsync();
        }

        /// <inheritdoc />
        public Task UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            var res = ApplySpecification(specification);
            return await res.CountAsync();
        }

        protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            var evaluator = new SpecificationEvaluator<T>();
            var entities = ApplyQueryFilter(Set);
            return evaluator.GetQuery(entities, specification);
        }

        protected IQueryable<T> ApplyQueryFilter(IQueryable<T> entities)
        {
            if (_filter.IsEnabled<ISoftDelete>() && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
                // ReSharper disable once SuspiciousTypeConversion.Global
                return entities.Where(v => ((ISoftDelete) v).IsDeleted == false);

            return entities;
        }

        protected T ApplyQueryFilter(T entity)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (entity is ISoftDelete softDelete && _filter.IsEnabled<ISoftDelete>())
                return softDelete.IsDeleted ? null : entity;
            return entity;
        }
    }
}