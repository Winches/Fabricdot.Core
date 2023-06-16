namespace Fabricdot.Infrastructure.Uow.Abstractions;

public interface ISupportSaveChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
