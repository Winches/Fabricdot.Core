using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Domain.Auditing;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SoftDelete_Tests : EntityFrameworkCoreTestsBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IDataFilter _dataFilter;

        public SoftDelete_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _authorRepository = provider.GetRequiredService<IAuthorRepository>();
            _dataFilter = provider.GetRequiredService<IDataFilter>();
        }

        [Fact]
        public async Task DeleteAsync_GivenISoftDelete_SoftDelete()
        {
            const int authorId = 1;
            await UseUowAsync(async () =>
            {
                var author = await _authorRepository.GetByIdAsync(authorId);
                await _authorRepository.DeleteAsync(author);
            });

            var author = await _authorRepository.GetByIdAsync(authorId);
            Assert.Null(author);
            using var scope = _dataFilter.Disable<ISoftDelete>();
            var author2 = await _authorRepository.GetByIdAsync(authorId);
            Assert.NotNull(author2);
            Assert.True(author2.IsDeleted);
        }

        [Fact]
        public async Task HardDeleteAsync_GivenISoftDelete_HardDelete()
        {
            const int authorId = 1;
            await UseUowAsync(async () =>
            {
                var author = await _authorRepository.GetByIdAsync(authorId);
                await _authorRepository.HardDeleteAsync(author);
            });

            using var scope = _dataFilter.Disable<ISoftDelete>();
            var author = await _authorRepository.GetByIdAsync(authorId);
            Assert.Null(author);
        }

        private async Task UseUowAsync(Func<Task> func)
        {
            var uowMgr = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            using var uow = uowMgr.Begin(requireNew: true);
            await func();
            await uow.CommitChangesAsync();
        }
    }
}