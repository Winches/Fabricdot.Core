using System.Threading.Tasks;
using IntegrationTests.Data.Entities;
using IntegrationTests.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Data.Tests
{
    public class EfRepositoryTest : TestBase
    {
        private readonly IBookRepository _bookRepository;

        public EfRepositoryTest()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task TestAdd()
        {
            const string id = "A4C59043-DBEC-4A48-919C-3985809AB6F2";
            var book = new Book(id, "Python");
            var ret = await _bookRepository.AddAsync(book);
            await UnitOfWork.CommitChangesAsync();

            Assert.Equal(ret, book);
            Assert.Equal(book.Id, id);
        }

        [Fact]
        public async Task TestUpdate()
        {
            var book = await _bookRepository.GetByNameAsync("CPP");
            const string name = "CPPV2";
            book.ChangeName(name);
            await _bookRepository.UpdateAsync(book);
            await UnitOfWork.CommitChangesAsync();
            var reloadEntity = await _bookRepository.GetByNameAsync(name);

            Assert.Equal(book.Name, name);
            Assert.Equal(book, reloadEntity);
        }

        [Fact]
        public async Task TestDelete()
        {
            var book1 = await _bookRepository.GetByNameAsync("CPP");
            await _bookRepository.DeleteAsync(book1);
            await UnitOfWork.CommitChangesAsync();
            var book2 = await _bookRepository.GetByNameAsync("CPP");

            Assert.Null(book2);
        }

        [Fact]
        public async Task TestGetById()
        {
            var book1 = await _bookRepository.GetByNameAsync("CPP");
            var book2 = await _bookRepository.GetByIdAsync(book1.Id);
            Assert.NotNull(book2);
            Assert.Equal(book1.Id, book2.Id);
        }
    }
}