using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

[IgnoreDependency]
public class EfRepository<TDbContext, T, TKey> : RepositoryBase<T, TKey>, IUnitOfWorkManagerAccessor
    where TDbContext : DbContext
    where T : class, IAggregateRoot, Fabricdot.Domain.Entities.IEntity<TKey>
    where TKey : notnull
{
    protected readonly ISpecificationEvaluator SpecificationEvaluator = new SpecificationEvaluator();
    protected readonly IDbContextProvider<TDbContext> DbContextProvider;

    public IUnitOfWorkManager UnitOfWorkManager => DbContextProvider.UnitOfWorkManager;

    public EfRepository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        DbContextProvider = dbContextProvider;
    }

    /// <inheritdoc />
    public override async Task<T> AddAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));

        var context = await GetDbContextAsync(cancellationToken);
        await context.AddAsync(entity, cancellationToken);
        return entity;
    }

    /// <inheritdoc />
    public override async Task DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));

        var context = await GetDbContextAsync(cancellationToken);
        context.Remove(entity);
    }

    /// <inheritdoc />
    public override async Task<T> GetByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        // Id of GUID type will become binary parameter when use MySql.Data driver https://stackoverflow.com/questions/65503169/entity-framework-core-generate-wrong-guid-parameter-with-mysql
        Guard.Against.Null(id, nameof(id));

        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        return await queryable.SingleOrDefaultAsync(v => v.Id.Equals(id), cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<T> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(specification, nameof(specification));

        var queryable = await GetQueryableAsync(specification, cancellationToken: cancellationToken);
        return await queryable.SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<IReadOnlyList<T>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        return await queryable.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(specification, nameof(specification));

        var queryable = await GetQueryableAsync(specification, cancellationToken: cancellationToken);
        return await queryable.ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));

        var context = await GetDbContextAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Modified;
    }

    /// <inheritdoc />
    public override async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        return await queryable.CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(specification, nameof(specification));

        var queryable = await GetQueryableAsync(specification, true, cancellationToken);
        return await queryable.CountAsync(cancellationToken);
    }

    protected virtual async Task<DbContext> GetDbContextAsync(CancellationToken cancellationToken)
    {
        return await DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<T>> GetQueryableAsync(
        ISpecification<T>? specification = null,
        bool evaluateCriteriaOnly = false,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync(cancellationToken);
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
        // Method won't evaluate Take, Skip, Ordering, and Include expressions in the specification
        // when 'evaluateCriteriaOnly' is true. https://github.com/ardalis/Specification/issues/134
        return SpecificationEvaluator.GetQuery(queryable, specification, evaluateCriteriaOnly);
    }

    protected virtual IQueryable<T> ApplyQueryFilter(IQueryable<T> queryable) => queryable;
}