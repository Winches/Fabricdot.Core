using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SoftDelete_Cascading_Tests : EfRepositoryTestsBase
    {
        private readonly IDataFilter _dataFilter;

        public SoftDelete_Cascading_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _dataFilter = provider.GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async Task DbContextBase_RemoveEntityOfCascadingCollection_SoftDelete()
        {
            var book = await BuildBookAsync();
            var tagName = book.Tags.First().Name;
            book.RemoveTag(tagName);
            FakeDbContext.Update(book);
            await FakeDbContext.SaveChangesAsync();

            var retrievalBook = await GetBookWithDetails(book.Id);
            var tags = retrievalBook.Tags;
            var tag = tags.SingleOrDefault(v => v.Name == tagName);
            Assert.NotNull(tag);
            Assert.True(tag.IsDeleted);
            Assert.DoesNotContain(tags, v => v.Name != tagName && v.IsDeleted);
        }

        [Fact]
        public async Task DbContextBase_RemoveCascadingObject_SoftDelete()
        {
            var book = await BuildBookAsync();
            book.Contents = null;
            FakeDbContext.Update(book);
            await FakeDbContext.SaveChangesAsync();

            var retrievalBook = await GetBookWithDetails(book.Id);
            var contents = retrievalBook.Contents;
            Assert.NotNull(contents);
            Assert.True(contents.IsDeleted);
        }

        [Fact]
        public async Task DbContextBase_RemovePrincpalEntity_KeepCascadingEntitiesState()
        {
            var book = await BuildBookAsync();
            var tagName = book.Tags.First().Name;
            book.RemoveTag(tagName);
            FakeDbContext.Remove(book);
            await FakeDbContext.SaveChangesAsync();

            var retrievalBook = await GetBookWithDetails(book.Id);
            var tags = retrievalBook.Tags;
            var tag = tags.SingleOrDefault(v => v.Name == tagName);
            var contents = retrievalBook.Contents;

            Assert.NotNull(retrievalBook);
            Assert.True(retrievalBook.IsDeleted);
            Assert.True(tag.IsDeleted);
            Assert.DoesNotContain(tags, v => v.Name != tagName && v.IsDeleted);
            Assert.NotNull(contents);
            Assert.False(contents.IsDeleted);
        }

        private async Task<Book> BuildBookAsync()
        {
            var book = new Book(
                Guid.NewGuid().ToString(),
                this.GetType().Name,
                new[] { "Tag1", "Tag2", "Tag3" });
            book.Contents = new BookContents("Introduce something.");
            await FakeDbContext.AddAsync(book);
            await FakeDbContext.SaveChangesAsync();

            return book;
        }

        private async Task<Book> GetBookWithDetails(string bookId)
        {
            using var scope = _dataFilter.Disable<ISoftDelete>();
            return await FakeDbContext.Set<Book>()
                                      .Include(v => v.Tags)
                                      .Include(v => v.Contents)
                                      .SingleOrDefaultAsync(v => v.Id == bookId);
        }
    }
}