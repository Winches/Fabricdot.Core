using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Domain.Services;

public static class RepositoryExtensions
{
    public static async Task HardDeleteAsync<T>(
        this IRepository<T> repository,
        T entity,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        Guard.Against.Null(repository, nameof(repository));
        Guard.Against.Null(entity, nameof(entity));

        if (repository is not IUnitOfWorkManagerAccessor unitOfWorkManagerAccessor)
        {
            throw new InvalidOperationException($"{repository.GetType().PrettyPrint()} do not implement interface {typeof(IUnitOfWorkManagerAccessor).PrettyPrint()}");
        }

        var unitOfWorkManager = unitOfWorkManagerAccessor.UnitOfWorkManager;
        if (unitOfWorkManager == null)
            throw new InvalidOperationException("There is no available unit-of-work manager.");

        if (unitOfWorkManager.Available != null)
        {
            unitOfWorkManager.Available.MarkHardDeletedEntity(entity);
            await repository.DeleteAsync(entity, cancellationToken);
        }
        else
        {
            using var uow = unitOfWorkManager.Begin();
            uow.MarkHardDeletedEntity(entity);
            await repository.DeleteAsync(entity, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);
        }
    }
}