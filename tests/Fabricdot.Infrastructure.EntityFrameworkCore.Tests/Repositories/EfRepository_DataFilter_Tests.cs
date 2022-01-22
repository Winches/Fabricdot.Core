using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EfRepository_DataFilter_Tests : EfRepositoryTestsBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IDataFilter _dataFilter;

        public EfRepository_DataFilter_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();
            _dataFilter = provider.GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async Task DeleteAsync_GivenSoftDeletedWhenDisableSoftDelete_PhysicalDelete()
        {
            const int authorId = 1;
            var author = await _authorRepository.GetByIdAsync(authorId);

            using var scope = _dataFilter.Disable<ISoftDelete>();
            await _authorRepository.DeleteAsync(author);

            var retrievalAuthor = await _authorRepository.GetByIdAsync(authorId);
            Assert.Null(retrievalAuthor);
        }

        [Fact]
        public async Task DeleteAsync_GivenISoftDeleteWhenEnableSoftDelete_SoftDelete()
        {
            const int authorId = 1;
            var author = await _authorRepository.GetByIdAsync(authorId);
            using var scope = _dataFilter.Enable<ISoftDelete>();
            await _authorRepository.DeleteAsync(author);

            var retrievalAuthor = await _authorRepository.GetByIdAsync(authorId);
            Assert.Null(retrievalAuthor);
        }

        [Fact]
        public async Task GetByIdAsync_GivenSoftDeletedWhenDisableSoftDelete_ReturnEntity()
        {
            using var scope = _dataFilter.Disable<ISoftDelete>();
            var actual = await _authorRepository.GetByIdAsync(FakeDataBuilder.DeletedAuthorId);
            Assert.NotNull(actual);
            Assert.True(actual.IsDeleted);
        }

        [Fact]
        public async Task GetByIdAsync_GivenSoftDeletedWhenEnableSoftDelete_ReturnNull()
        {
            using var scope = _dataFilter.Enable<ISoftDelete>();
            var actual = await _authorRepository.GetByIdAsync(FakeDataBuilder.DeletedAuthorId);
            Assert.Null(actual);
        }
    }
}