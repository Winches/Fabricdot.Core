using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    internal class BookRepository : EfRepository<FakeDbContext, Book, string>, IBookRepository
    {
        /// <inheritdoc />
        public BookRepository(IDbContextProvider<FakeDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        /// <inheritdoc />
        public async Task<Book> GetByNameAsync(string name)
        {
            var queryable = await GetQueryableAsync();
            return await queryable.FirstOrDefaultAsync(v => v.Name == name);
        }
    }
}