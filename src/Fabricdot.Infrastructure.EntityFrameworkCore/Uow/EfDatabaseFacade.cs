using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Uow;

public class EfDatabaseFacade : IDatabaseFacade, ISupportSaveChanges
{
    public DbContext DbContext { get; }

    public EfDatabaseFacade(DbContext dbContext)
    {
        Guard.Against.Null(dbContext, nameof(dbContext));
        DbContext = dbContext;
    }

    /// <inheritdoc />
    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
