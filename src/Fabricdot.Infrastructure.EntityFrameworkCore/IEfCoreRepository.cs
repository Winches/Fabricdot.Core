using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

public interface IEfCoreRepository<TEntity>
{
    Task<DbContext> GetDbContextAsync(CancellationToken cancellationToken = default);

    Task<IQueryable<TEntity>> GetQueryableAsync(CancellationToken cancellationToken = default);

    IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> queryable);
}
