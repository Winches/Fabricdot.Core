using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_UpdateAsync_Tests : EntityFrameworkCoreTestsBase
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
            var book = await DbContext.Set<Book>().FirstOrDefaultAsync(v => v.Name == "CSharp");
            var expected = "CSharpV2";
            book.ChangeName(expected);
            await _bookRepository.UpdateAsync(book);
            await UnitOfWork.CommitChangesAsync();

            var retrievalBook = await DbContext.FindAsync<Book>(book.Id);
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
            async Task Func()
            {
                await _bookRepository.UpdateAsync(null);
            }
            await Assert.ThrowsAsync<ArgumentNullException>(Func);
        }
    }
}