using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_DeleteRangeAsync_Tests : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public EfRepository_DeleteRangeAsync_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();
        }

        [Fact]
        public async Task DeleteRangeAsync_GivenSavedEntitiesWithoutISoftDeleted_PhysicalDelete()
        {
            var books = FakeDbContext.Set<Book>().Where(v => new[] { "1", "2" }.Contains(v.Id));

            await _bookRepository.DeleteRangeAsync(books);
            await FakeDbContext.SaveChangesAsync();

            var deletedBooks = FakeDbContext.Set<Book>().Where(v => new[] { "1", "2" }.Contains(v.Id));
            Assert.Empty(deletedBooks);
        }

        [Fact]
        public async Task DeleteRangeAsync_GivenSavedEntitiesWithISoftDeleted_SoftDelete()
        {
            var authors = FakeDbContext.Set<Author>().Where(v => new[] { 1, 2 }.Contains(v.Id));

            await _authorRepository.DeleteRangeAsync(authors);
            await FakeDbContext.SaveChangesAsync();

            var deletedAuthors = FakeDbContext.Set<Author>().Where(v => new[] { 1, 2 }.Contains(v.Id));
            Assert.NotEmpty(deletedAuthors);
            foreach (var deletedAuthor in deletedAuthors)
                Assert.True(deletedAuthor.IsDeleted);
        }

        [Fact]
        public async Task DeleteRangeAsync_GivenNull_ThrowException()
        {
            async Task Func()
            {
                await _authorRepository.DeleteRangeAsync(null);
                await FakeDbContext.SaveChangesAsync();
            }

            await Assert.ThrowsAsync<ArgumentNullException>(Func);
        }
    }
}