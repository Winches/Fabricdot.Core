using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Services
{
    public interface IRepository : IHasUnitOfWorkScope
    {
    }

    public interface IRepository<T> : IReadOnlyRepository<T> where T : IAggregateRoot
    {
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }

    public interface IRepository<T, in TKey> : IRepository<T>, IReadOnlyRepository<T, TKey> where T : IAggregateRoot, IEntity<TKey>
    {
    }
}