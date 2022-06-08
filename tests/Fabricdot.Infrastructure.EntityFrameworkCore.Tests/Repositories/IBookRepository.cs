using System.Threading.Tasks;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public interface IBookRepository : IRepository<Book, string>
{
    Task<Book> GetByNameAsync(string name);
}