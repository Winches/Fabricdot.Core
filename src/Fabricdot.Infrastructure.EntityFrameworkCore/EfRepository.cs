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
public class EfRepository<TDbContext, T, TKey> : RepositoryBase<T, TKey>, IEfCoreRepository<T>, IUnitOfWorkManagerAccessor
    where TDbContext : DbContext
    where T : class, IAggregateRoot, Fabricdot.Domain.Entities.IEntity<TKey>
    where TKey : notnull
{
    private readonly ISpecificationEvaluator _specificationEvaluator;

    public IUnitOfWorkManager UnitOfWorkManager => DbContextProvider.UnitOfWorkManager;

    protected IDbContextProvider<TDbContext> DbContextProvider { get; }

    public EfRepository(IDbContextProvider<TDbContext> dbContextProvider)
    {
        DbContextProvider = dbContextProvider;
        _specificationEvaluator = SpecificationEvaluator.Default;
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
    public override async Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(entity, nameof(entity));

        var context = await GetDbContextAsync(cancellationToken);
        context.Entry(entity).State = EntityState.Modified;
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
    public override async Task<T?> GetByIdAsync(
        TKey key,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        // Id of GUID type will become binary parameter when use MySql.Data driver https://stackoverflow.com/questions/65503169/entity-framework-core-generate-wrong-guid-parameter-with-mysql
        Guard.Against.Null(key, nameof(key));

        var queryable = await GetQueryableAsync(cancellationToken);
        if (includeDetails)
            queryable = IncludeDetails(queryable);
        return await queryable.SingleOrDefaultAsync(v => v.Id.Equals(key), cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        return await GetAsync(specification, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<T?> GetAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(specification, nameof(specification));

        var queryable = await GetQueryableAsync(specification, cancellationToken: cancellationToken);
        return await queryable.SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<IReadOnlyList<T>> ListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            queryable = IncludeDetails(queryable);
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
    public override async Task<IReadOnlyList<T>> ListAsync(
        IEnumerable<TKey> keys,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.NullOrEmpty(keys, nameof(keys));
        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            queryable = IncludeDetails(queryable);
        return await queryable.Where(v => keys.Contains(v.Id)).ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken: cancellationToken);
        return await queryable.LongCountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<long> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(specification, nameof(specification));

        var queryable = await GetQueryableAsync(specification, true, cancellationToken);
        return await queryable.LongCountAsync(cancellationToken);
    }

    public virtual async Task<DbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        return await DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    public virtual async Task<IQueryable<T>> GetQueryableAsync(CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync(cancellationToken);
        return context.Set<T>();
    }

    public virtual IQueryable<T> IncludeDetails(IQueryable<T> queryable) => queryable;

    protected virtual async Task<IQueryable<T>> GetQueryableAsync(
            ISpecification<T>? specification = null,
        bool evaluateCriteriaOnly = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync(cancellationToken);
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
        return _specificationEvaluator.GetQuery(queryable, specification, evaluateCriteriaOnly);
    }
}
