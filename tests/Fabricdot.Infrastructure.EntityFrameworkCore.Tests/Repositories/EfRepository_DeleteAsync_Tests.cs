using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_DeleteAsync_Tests : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public EfRepository_DeleteAsync_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();
        }

        [Fact]
        public async Task DeleteAsync_GivenEntityWithoutISoftDeleted_PhysicalDelete()
        {
            var book = await FakeDbContext.Set<Book>().FirstOrDefaultAsync(v => v.Name == "Java");
            await _bookRepository.DeleteAsync(book);
            await FakeDbContext.SaveChangesAsync();

            var deletedBook = await FakeDbContext.FindAsync<Book>(book.Id);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task DeleteAsync_GivenEntityWithISoftDeleted_SoftDelete()
        {
            var author = await FakeDbContext.FindAsync<Author>(1);
            await _authorRepository.DeleteAsync(author);
            await FakeDbContext.SaveChangesAsync();

            var deletedAuthor = await FakeDbContext.FindAsync<Author>(author.Id);
            Assert.NotNull(deletedAuthor);
            Assert.True(deletedAuthor.IsDeleted);
        }

        [Fact]
        public async Task DeleteAsync_GivenUnsavedEntity_ThrowException()
        {
            var book = new Book("10", "Python");

            async Task Func()
            {
                await _bookRepository.DeleteAsync(book);
                await FakeDbContext.SaveChangesAsync();
            }

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(Func);
        }

        [Fact]
        public async Task DeleteAsync_GivenNull_ThrowException()
        {
            async Task Func()
            {
                await _bookRepository.DeleteAsync(null);
                await FakeDbContext.SaveChangesAsync();
            }

            await Assert.ThrowsAsync<ArgumentNullException>(Func);
        }
    }
}