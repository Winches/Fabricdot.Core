using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Data.Filters;
using IntegrationTests.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Data.Tests
{
    public class SoftDeleteTest : TestBase
    {
        private readonly IBookRepository _bookRepository;

        public SoftDeleteTest()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task TestSoftDelete()
        {
            const string bookName = "Rust";
            var book = await _bookRepository.GetByNameAsync(bookName);
            await _bookRepository.DeleteAsync(book);
            await UnitOfWork.CommitChangesAsync();
            var target = await _bookRepository.GetByNameAsync(bookName);
            Assert.Null(target);
        }

        [Fact]
        public async Task TestGetSoftDeletedEntityInScope()
        {
            const string bookName = "Java";
            var book = await _bookRepository.GetByNameAsync(bookName);
            await _bookRepository.DeleteAsync(book);
            await UnitOfWork.CommitChangesAsync();

            using var scope = ServiceScope.ServiceProvider.GetRequiredService<IDataFilter>().Disable<ISoftDelete>();
            var target = await _bookRepository.GetByNameAsync(bookName);
            Assert.NotNull(target);
            Assert.True(target.IsDeleted);
        }

        [Fact]
        public async Task TestGetSoftDeletedEntityOutScope()
        {
            const string bookName = "Typescript";
            var book = await _bookRepository.GetByNameAsync(bookName);
            await _bookRepository.DeleteAsync(book);
            await UnitOfWork.CommitChangesAsync();

            var provider = ServiceScope.ServiceProvider;
            using (var _ = provider.GetRequiredService<IDataFilter>().Disable<ISoftDelete>())
            {
            }

            var target = await _bookRepository.GetByNameAsync(bookName);
            Assert.Null(target);
        }
    }
}