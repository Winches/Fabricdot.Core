using System;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public class ConcurrencyStampTest : EfRepositoryTestsBase
    {
        private readonly IBookRepository _bookRepository;

        public ConcurrencyStampTest()
        {
            var provider = ServiceScope.ServiceProvider;
            _bookRepository = provider.GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task UpdateAsync_WhenEntityChanged_ThrowException()
        {
            var book1 = await _bookRepository.GetByNameAsync("CSharp");
            book1.ChangeName("CSharp1");

            var book2 = await _bookRepository.GetByNameAsync("CSharp");
            book2.ConcurrencyStamp = Guid.NewGuid().ToString("N");

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await _bookRepository.UpdateAsync(book1);
                await FakeDbContext.SaveChangesAsync();
            });
        }

        [Fact]
        public async Task DeleteAsync_WhenEntityChanged_ThrowException()
        {
            var book1 = await _bookRepository.GetByNameAsync("CSharp");

            var book2 = await _bookRepository.GetByNameAsync("CSharp");
            book2.ConcurrencyStamp = Guid.NewGuid().ToString("N");

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await _bookRepository.DeleteAsync(book1);
                await FakeDbContext.SaveChangesAsync();
            });
        }
    }
}