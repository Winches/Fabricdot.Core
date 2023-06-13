using System.Linq.Expressions;
using Ardalis.GuardClauses;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Identity.Infrastructure.Data.Repositories;

public class UserRepository<TDbContext, TUser> : EfRepository<TDbContext, TUser, Guid>, IUserRepository<TUser>, ISupportExplicitLoading<TUser>
    where TDbContext : DbContextBase
    where TUser : IdentityUser
{
    public UserRepository(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    [Obsolete("Use 'GetByIdAsync'")]
    public virtual async Task<TUser?> GetDetailsByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await GetByIdAsync(id, cancellationToken: cancellationToken);
    }

    public virtual async Task<TUser?> GetByLoginAsync(
        string loginProvider,
        string providerKey,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            query = IncludeDetails(query);
        return await query.Where(v => v.Logins.Any(o => o.LoginProvider == loginProvider && o.ProviderKey == providerKey))
                          .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TUser?> GetByNormalizedEmailAsync(
        string normalizedEmail,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            query = IncludeDetails(query);
        return await query.Where(v => v.NormalizedEmail == normalizedEmail)
                          .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TUser?> GetByNormalizedUserNameAsync(
        string normalizedUserName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            query = IncludeDetails(query);
        return await query.Where(v => v.NormalizedUserName == normalizedUserName)
                          .SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<string>> ListRoleNamesAsync<TRole>(
        Guid id,
        CancellationToken cancellationToken = default) where TRole : IdentityRole
    {
        var dbContext = await GetDbContextAsync(cancellationToken);
        var query = from userRole in dbContext.Set<IdentityUserRole>()
                    join role in dbContext.Set<TRole>() on userRole.RoleId equals role.Id
                    where userRole.UserId == id
                    select role.Name;
        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    //public virtual async Task<IReadOnlyCollection<TUser>> ListAsync(
    //    ICollection<Guid> ids,
    //    bool includeDetails = false,
    //    CancellationToken cancellationToken = default)
    //{
    //    var query = await GetQueryableAsync(includeDetails, cancellationToken: cancellationToken);
    //    return await query.Where(v => ids.Contains(v.Id))
    //                      .ToListAsync(cancellationToken);
    //}

    public virtual async Task<IReadOnlyCollection<TUser>> ListByClaimAsync(
        string claimType,
        string claimValue,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            query = IncludeDetails(query);
        return await query.Where(v => v.Claims.Any(o => o.ClaimType == claimType && o.ClaimValue == claimValue))
                          .ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<TUser>> ListByRoleIdAsync(
        Guid roleId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync(cancellationToken: cancellationToken);
        if (includeDetails)
            query = IncludeDetails(query);
        return await query.Where(v => v.Roles.Any(o => o.RoleId == roleId))
                          .ToListAsync(cancellationToken);
    }

    public virtual async Task LoadReferenceAsync<TProperty>(
        TUser entity,
        Expression<Func<TUser, TProperty>> propertyExpression,
        CancellationToken cancellationToken) where TProperty : class
    {
        Guard.Against.Null(entity, nameof(entity));
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var dbContext = await GetDbContextAsync(cancellationToken);
        await dbContext.Entry(entity)
                       .Reference(propertyExpression!)
                       .LoadAsync(cancellationToken);
    }

    public virtual async Task LoadCollectionAsync<TProperty>(
        TUser entity,
        Expression<Func<TUser, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken) where TProperty : class
    {
        Guard.Against.Null(entity, nameof(entity));
        Guard.Against.Null(propertyExpression, nameof(propertyExpression));

        var dbContext = await GetDbContextAsync(cancellationToken);
        await dbContext.Entry(entity)
                       .Collection(propertyExpression)
                       .LoadAsync(cancellationToken);
    }

    //protected virtual async Task<IQueryable<TUser>> GetQueryableAsync(
    //    bool includeDetails,
    //    bool evaluateCriteriaOnly = false,
    //    CancellationToken cancellationToken = default)
    //{
    //    return await base.GetQueryableAsync(
    //        new UserWithDetailsSpecification<TUser>(includeDetails),
    //        evaluateCriteriaOnly,
    //        cancellationToken);
    //}

    /// <summary>
    ///     Include claims,logins,tokens and roles.
    /// </summary>
    /// <param name="queryable"></param>
    /// <returns></returns>
    public override IQueryable<TUser> IncludeDetails(IQueryable<TUser> queryable)
    {
        return queryable.Include(v => v.Claims)
                        .Include(v => v.Logins)
                        .Include(v => v.Tokens)
                        .Include(v => v.Roles);
    }
}
