using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Services
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }

    public interface IRepository<T, in TKey> : IRepository<T> where T : IAggregateRoot, Entities.IEntity<TKey>
    {
        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}