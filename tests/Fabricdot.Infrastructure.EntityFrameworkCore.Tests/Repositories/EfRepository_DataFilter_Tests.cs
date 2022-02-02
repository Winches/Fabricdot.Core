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