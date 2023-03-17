using Ardalis.GuardClauses;
using Fabricdot.Domain.Entities;
using Fabricdot.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore;

public static class EfRepositoryExtensions
{
    public static IEfCoreRepository<T> ToEfRepository<T>(this IReadOnlyRepository<T> readOnlyRepository) where T : IAggregateRoot
    {
        Guard.Against.Null(readOnlyRepository, nameof(readOnlyRepository));

        return readOnlyRepository is IEfCoreRepository<T> efCoreRepository
            ? efCoreRepository
            : throw new InvalidCastException($"'{readOnlyRepository.GetType().PrettyPrint()}' does not inherit the 'IEfCoreRepository<T>'");
    }

    public static Task<DbContext> GetDbContextAsync<T>(
        this IReadOnlyRepository<T> readOnlyRepository,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        var efCoreRepository = readOnlyRepository.ToEfRepository();
        return efCoreRepository.GetDbContextAsync(cancellationToken);
    }

    public static Task<IQueryable<T>> GetQueryableAsync<T>(
        this IReadOnlyRepository<T> readOnlyRepository,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        var efCoreRepository = readOnlyRepository.ToEfRepository();
        return efCoreRepository.GetQueryableAsync(cancellationToken);
    }

    public static async Task<IQueryable<T>> GetQueryableAsync<T>(
        this IReadOnlyRepository<T> readOnlyRepository,
        bool includeDetails = false,
        CancellationToken cancellationToken = default) where T : IAggregateRoot
    {
        var efCoreRepository = readOnlyRepository.ToEfRepository();
        var queryable = await efCoreRepository.GetQueryableAsync(cancellationToken);

        return includeDetails ? efCoreRepository.IncludeDetails(queryable) : queryable;
    }
}