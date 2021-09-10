using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_AddAsync_Tests : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;

        public EfRepository_AddAsync_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task AddAsync_GivenUnsavedEntity_Correctly()
        {
            var id = Guid.NewGuid().ToString();
            var book = new Book(id, "Python");
            var ret = await _bookRepository.AddAsync(book);
            await FakeDbContext.SaveChangesAsync();

            var retrievalBook = await FakeDbContext.FindAsync<Book>(id);
            var findId = retrievalBook.Id;

            Assert.Same(book, ret);
            Assert.Equal(id, findId);
        }

        [Fact]
        public async Task AddAsync_GivenSavedEntity_ThrowException()
        {
            async Task Func()
            {
                var book = await FakeDbContext.Set<Book>().FirstOrDefaultAsync(v => v.Name == "CSharp");
                await _bookRepository.AddAsync(book);
                await FakeDbContext.SaveChangesAsync();
            }

            await Assert.ThrowsAsync<DbUpdateException>(Func);
        }

        [Fact]
        public async Task AddAsync_GivenNull_ThrowException()
        {
            async Task Func()
            {
                await _bookRepository.AddAsync(null);
                await FakeDbContext.SaveChangesAsync();
            }

            await Assert.ThrowsAsync<ArgumentNullException>(Func);
        }
    }
}