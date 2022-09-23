using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Domain.Specifications;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Infrastructure.Data.Repositories;

public class RoleRepository<TDbContext, TRole> : EfRepository<TDbContext, TRole, Guid>, IRoleRepository<TRole>, ISupportExplicitLoading<TRole>
    where TDbContext : DbContextBase
    where TRole : IdentityRole
{
    public RoleRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<TRole?> GetDetailsByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(true, cancellationToken: cancellationToken);
        return await query.Where(v => v.Id == id)
                          .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TRole?> GetByNormalizedNameAsync(
        string normalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(includeDetails, cancellationToken: cancellationToken);
        return await query.Where(v => v.NormalizedName == normalizedName)
                          .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<TRole>> ListAsync(
        ICollection<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(includeDetails, cancellationToken: cancellationToken);
        return await query.Where(v => ids.Contains(v.Id))
                          .ToListAsync(cancellationToken);
    }

    async Task ISupportExplicitLoading<TRole>.LoadReferenceAsync<TProperty>(
        TRole entity,
        Expression<Func<TRole, TProperty>> propertyExpression,
        CancellationToken cancellationToken) where TProperty : class
    {
        Guard.Against.Null(entity, nameof(entity));
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var dbContext = await GetDbContextAsync(cancellationToken);
        await dbContext.Entry(entity)
                       .Reference(propertyExpression!)
                       .LoadAsync(cancellationToken);
    }

    async Task ISupportExplicitLoading<TRole>.LoadCollectionAsync<TProperty>(
        TRole entity,
        Expression<Func<TRole, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken) where TProperty : class
    {
        Guard.Against.Null(entity, nameof(entity));
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var dbContext = await GetDbContextAsync(cancellationToken);
        await dbContext.Entry(entity)
                       .Collection(propertyExpression)
                       .LoadAsync(cancellationToken);
    }

    protected virtual async Task<IQueryable<TRole>> GetQueryableAsync(
        bool includeDetails,
        bool evaluateCriteriaOnly = false,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryableAsync(
            new RoleWithDetailsSpecification<TRole>(includeDetails),
            evaluateCriteriaOnly,
            cancellationToken: cancellationToken);
    }
}