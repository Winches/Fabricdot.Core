using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Domain.Core.Services
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        public Task<T> GetBySpecAsync(ISpecification<T> specification);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<IReadOnlyList<T>> ListAllAsync(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specification);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        public Task DeleteRangeAsync(IEnumerable<T> entities);
    }

    public interface IRepository<T, in TKey> : IRepository<T> where T : IAggregateRoot, Entities.IEntity<TKey>
    {
        Task<T> GetByIdAsync(TKey id);
    }
}