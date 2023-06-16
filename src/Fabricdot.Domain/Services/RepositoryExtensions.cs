using Ardalis.GuardClauses;
using Ardalis.Specification;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Services;

public static class RepositoryExtensions
{
    public static async Task<bool> AnyAsync<T>(
        this IReadOnlyRepository<T> repository,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        Guard.Against.Null(repository, nameof(repository));
        return await repository.CountAsync(cancellationToken) > 0;
    }

    public static async Task<bool> AnyAsync<T>(
        this IReadOnlyRepository<T> repository,
        ISpecification<T> specification,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        Guard.Against.Null(repository, nameof(repository));
        return await repository.CountAsync(specification, cancellationToken) > 0;
    }
}
