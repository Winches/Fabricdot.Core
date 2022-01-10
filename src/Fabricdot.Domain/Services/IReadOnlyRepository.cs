using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Services
{
    public interface IReadOnlyRepository<T> : IRepository where T : IAggregateRoot
    {
        public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        Task<long> CountAsync(CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    }

    public interface IReadOnlyRepository<T, in TKey> : IReadOnlyRepository<T> where T : IAggregateRoot
    {
        Task<T> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}