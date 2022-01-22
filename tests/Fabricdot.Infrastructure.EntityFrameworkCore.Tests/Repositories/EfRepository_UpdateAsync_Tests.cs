using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_UpdateAsync_Tests : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;

        public EfRepository_UpdateAsync_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task UpdateAsync_GivenSavedEntity_SaveChanges()
        {
            const string bookName = "CSharp";
            const string expected = "CSharpV2";
            var book = await _bookRepository.GetByNameAsync(bookName);
            book.ChangeName(expected);
            await _bookRepository.UpdateAsync(book);

            var retrievalBook = await _bookRepository.GetByIdAsync(book.Id);
            var actual = retrievalBook.Name;
            Assert.Equal(expected, actual);
        }

        //[Fact]
        //public async Task UpdateAsync_GivenUnsavedEntity_ThrowException()
        //{
        //    var id = Guid.NewGuid().ToString();
        //    var book = new Book(id, "Python");

        //    async Task Func()
        //    {
        //        await _bookRepository.UpdateAsync(book);
        //    }
        //    await Assert.ThrowsAsync<DbUpdateException>(Func);
        //}

        [Fact]
        public async Task UpdateAsync_GivenNull_ThrowException()
        {
            async Task Func() => await _bookRepository.UpdateAsync(null);
            await Assert.ThrowsAsync<ArgumentNullException>(Func);
        }
    }
}