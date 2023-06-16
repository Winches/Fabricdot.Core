namespace Fabricdot.Infrastructure.Uow.Abstractions;

public interface ITransactionFacade : IDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}
