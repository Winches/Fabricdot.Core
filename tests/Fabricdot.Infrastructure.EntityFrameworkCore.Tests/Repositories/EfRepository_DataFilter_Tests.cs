using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_DataFilter_Tests : EfRepositoryTestsBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IDataFilter _dataFilter;
        private readonly Author _softDeletedAuthor;

        public EfRepository_DataFilter_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();
            _dataFilter = provider.GetRequiredService<IDataFilter>();

            _softDeletedAuthor = SoftDeleteAuthor();
        }

        [Fact]
        public async Task DeleteAsync_GivenSoftDeletedWhenDisableSoftDelete_PhysicalDelete()
        {
            const int authorId = 1;
            var author = await FakeDbContext.FindAsync<Author>(authorId);

            using var scope = _dataFilter.Disable<ISoftDelete>();
            await _authorRepository.DeleteAsync(author);
            await FakeDbContext.SaveChangesAsync();

            var retrievalAuthor = await FakeDbContext.FindAsync<Author>(authorId);
            Assert.Null(retrievalAuthor);
        }

        [Fact]
        public async Task DeleteAsync_GivenISoftDeleteWhenEnableSoftDelete_SoftDelete()
        {
            const int authorId = 1;
            var author = await FakeDbContext.FindAsync<Author>(authorId);
            using var scope = _dataFilter.Enable<ISoftDelete>();
            await _authorRepository.DeleteAsync(author);
            await FakeDbContext.SaveChangesAsync();

            var retrievalAuthor = await FakeDbContext.FindAsync<Author>(authorId);
            Assert.NotNull(retrievalAuthor);
            Assert.True(retrievalAuthor.IsDeleted);
        }

        [Fact]
        public async Task GetByIdAsync_GivenSoftDeletedWhenDisableSoftDelete_ReturnEntity()
        {
            var author = _softDeletedAuthor;
            using var scope = _dataFilter.Disable<ISoftDelete>();
            var actual = await _authorRepository.GetByIdAsync(author.Id);
            Assert.Equal(author, actual);
        }

        [Fact]
        public async Task GetByIdAsync_GivenSoftDeletedWhenEnableSoftDelete_ReturnNull()
        {
            var author = _softDeletedAuthor;
            using var scope = _dataFilter.Enable<ISoftDelete>();
            var actual = await _authorRepository.GetByIdAsync(author.Id);
            Assert.Null(actual);
        }

        private Author SoftDeleteAuthor()
        {
            var author = FakeDbContext.Find<Author>(2);
            author.MarkDeleted();
            FakeDbContext.Update(author);
            FakeDbContext.SaveChanges();
            return author;
        }
    }
}