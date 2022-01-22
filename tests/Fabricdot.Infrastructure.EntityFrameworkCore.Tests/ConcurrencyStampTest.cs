using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public class ConcurrencyStampTest : EntityFrameworkCoreTestsBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IDataFilter _dataFilter;

        public ConcurrencyStampTest()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
            _dataFilter = provider.GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async Task UpdateAsync_WhenEntityChanged_ThrowException()
        {
            const string bookName = "CSharp";
            var book1 = await _bookRepository.GetByNameAsync(bookName);
            book1.ChangeName("CSharp1");

            var book2 = await _bookRepository.GetByNameAsync(bookName);
            await _bookRepository.UpdateAsync(book2);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                using var scope = _dataFilter.Disable<ISoftDelete>();
                await _bookRepository.UpdateAsync(book1);
            });
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityChanged_ThrowException()
        {
            const string bookName = "CSharp";
            var book1 = await _bookRepository.GetByNameAsync(bookName);

            var book2 = await _bookRepository.GetByNameAsync(bookName);
            book2.ChangeName("CSharp2");
            await _bookRepository.UpdateAsync(book2);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                using var scope = _dataFilter.Disable<ISoftDelete>();
                await _bookRepository.DeleteAsync(book1);
            });
        }
    }
}