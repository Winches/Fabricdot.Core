using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    public class EfEntityChangeTrackerTests : EntityFrameworkCoreTestsBase
    {
        private readonly IEntityChangeTracker _changeTracker;

        public EfEntityChangeTrackerTests()
        {
            _changeTracker = ServiceScope.ServiceProvider.GetRequiredService<IEntityChangeTracker>();
        }

        [Fact]
        public async Task Entries_AddEntity_ReturnChanges()
        {
            var book = new Book("10", "Python");
            await DbContext.AddAsync(book);
            var entries = _changeTracker.Entries();
            var changeInfo = entries.Single();
            Assert.Equal(book, changeInfo.Entity);
            Assert.Equal(EntityStatus.Created, changeInfo.Status);
        }

        [Fact]
        public async Task Entries_UpdateEntity_ReturnChanges()
        {
            var book = await DbContext.FindAsync<Book>("1");
            book.ChangeName(book.Name + "V2");
            DbContext.Update(book);
            var entries = _changeTracker.Entries();
            var changeInfo = entries.Single();
            Assert.Equal(book, changeInfo.Entity);
            Assert.Equal(EntityStatus.Updated, changeInfo.Status);
        }

        [Fact]
        public async Task Entries_DeleteEntity_ReturnChanges()
        {
            var book = await DbContext.FindAsync<Book>("1");
            DbContext.Remove(book);
            var entries = _changeTracker.Entries();
            var changeInfo = entries.Single();
            Assert.Equal(book, changeInfo.Entity);
            Assert.Equal(EntityStatus.Deleted, changeInfo.Status);
        }
    }
}