using Ardalis.Specification;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Services;

public abstract class RepositoryBase<T> : IRepository<T> where T : IAggregateRoot
{
    /// <inheritdoc />
    public abstract Task<T> AddAsync(
        T entity,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task DeleteAsync(
        T entity,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<T?> GetBySpecAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<T?> GetAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<IReadOnlyList<T>> ListAsync(
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<long> CountAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<long> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken = default);
}

public abstract class RepositoryBase<T, TKey> : RepositoryBase<T>, IRepository<T, TKey> where T : IAggregateRoot, Entities.IEntity<TKey>
{
    /// <inheritdoc />
    public abstract Task<T?> GetByIdAsync(
        TKey key,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<IReadOnlyList<T>> ListAsync(
        IEnumerable<TKey> keys,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}