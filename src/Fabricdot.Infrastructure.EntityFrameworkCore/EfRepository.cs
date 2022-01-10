using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore
{
    public class EfRepository<TDbContext, T, TKey> : IRepository<T, TKey>
    where TDbContext : DbContext
    where T : class, IAggregateRoot, Fabricdot.Domain.Entities.IEntity<TKey>
    {
        private readonly ISpecificationEvaluator _specificationEvaluator = new SpecificationEvaluator();
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private IDataFilter _filter;

        protected EfRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        /// <inheritdoc />
        public virtual async Task<T> AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            await context.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            context.Remove(entity);
        }

        /// <inheritdoc />
        public virtual async Task DeleteRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            context.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            // Id of GUID type will become binary parameter when use MySql.Data driver https://stackoverflow.com/questions/65503169/entity-framework-core-generate-wrong-guid-parameter-with-mysql
            var queryable = await GetQueryableAsync();
            return await queryable.SingleOrDefaultAsync(v => v.Id.Equals(id), cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<T> GetBySpecAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(specification);
            return await queryable.SingleOrDefaultAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> ListAsync(
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<T>> ListAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(specification);
            return await queryable.ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual async Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.CountAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task<int> CountAsync(
            ISpecification<T> specification,
            CancellationToken cancellationToken = default)
        {
            var queryable = await GetQueryableAsync(specification, true);
            return await queryable.CountAsync(cancellationToken);
        }

        protected virtual async Task<DbContext> GetDbContextAsync()
        {
            var dbContext = await _dbContextProvider.GetDbContextAsync();
            _filter = dbContext.GetRequiredService<IDataFilter>();
            return dbContext;
        }

        protected virtual async Task<IQueryable<T>> GetQueryableAsync(
            ISpecification<T> specification = null,
            bool evaluateCriteriaOnly = false)
        {
            var context = await GetDbContextAsync();
            var queryable = ApplyQueryFilter(context.Set<T>());
            return specification == null
                ? queryable
                : ApplySpecification(queryable, specification, evaluateCriteriaOnly);
        }

        protected virtual IQueryable<T> ApplySpecification(
            IQueryable<T> queryable,
            ISpecification<T> specification,
            bool evaluateCriteriaOnly = false)
        {
            // Method won't evaluate Take, Skip, Ordering, and Include expressions in the
            // specification when 'evaluateCriteriaOnly' is true. https://github.com/ardalis/Specification/issues/134
            return _specificationEvaluator.GetQuery(queryable, specification, evaluateCriteriaOnly);
        }

        protected virtual IQueryable<T> ApplyQueryFilter(IQueryable<T> queryable)
        {
            //if (_filter.IsEnabled<ISoftDelete>() && typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            //    // ReSharper disable once SuspiciousTypeConversion.Global
            //    return queryable.Where(v => !((ISoftDelete)v).IsDeleted);

            return queryable;
        }
    }
}