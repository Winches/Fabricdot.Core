using System;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using IntegrationTests.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Data.Repositories
{
    public class BookRepository : EfRepository<Book, string>, IBookRepository
    {
        /// <inheritdoc />
        public BookRepository(FakeDbContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
        {
        }

        /// <inheritdoc />
        public async Task<Book> GetByNameAsync(string name)
        {
            return await ApplyQueryFilter(Context.Set<Book>()).FirstOrDefaultAsync(v => v.Name == name);
        }
    }
    public interface IBookRepository : IRepository<Book, string>
    {
        Task<Book> GetByNameAsync(string name);
    }
}