using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow;

public static class UnitOfWorkFacadeExtensions
{
    public static async Task SaveChangesAsync(
        this IUnitOfWorkFacade unitOfWorkFacade,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(unitOfWorkFacade, nameof(unitOfWorkFacade));
        if (unitOfWorkFacade is ISupportSaveChanges supportSaveChanges)
            await supportSaveChanges.SaveChangesAsync(cancellationToken);
    }
}