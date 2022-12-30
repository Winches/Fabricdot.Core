using Ardalis.Specification;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Services;

public interface IReadOnlyRepository<T> : IRepository where T : IAggregateRoot
{
    [Obsolete("Use 'GetAsync'")]
    public Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    public Task<T?> GetAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(CancellationToken cancellationToken = default);

    Task<long> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<T, in TKey> : IReadOnlyRepository<T> where T : IAggregateRoot
{
    Task<T?> GetByIdAsync(
        TKey key,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(
        IEnumerable<TKey> keys,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}