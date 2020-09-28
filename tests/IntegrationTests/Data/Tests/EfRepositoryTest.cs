using System.Threading.Tasks;
using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Data.Filters;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTests.Data.Tests
{
    public class EfRepositoryTest
    {
        private readonly IFakeRepository _fakeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EfRepositoryTest()
        {
            var provider = ContainerBuilder.GetServiceProvider();
            provider.GetRequiredService<FakeDbContext>().Database.EnsureCreated();
            _fakeRepository = provider.GetRequiredService<IFakeRepository>();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
        }

        [Fact]
        public async Task TestAdd()
        {
            const string id = "A4C59043-DBEC-4A48-919C-3985809AB6F2";
            var entity = new FakeEntity(id, "TestAdd");
            var ret = await _fakeRepository.AddAsync(entity);
            await _unitOfWork.CommitChangesAsync();
            Assert.Equal(ret, entity);
            Assert.Equal(entity.Id, id);
        }

        [Fact]
        public async Task TestUpdate()
        {
            const string id = "A4C59043-DBEC-4A48-919C-3985809AB6F2";
            var entity = await _fakeRepository.GetByIdAsync(id);
            const string name = "TestUpdate";
            entity.ChangeName(name);
            await _fakeRepository.UpdateAsync(entity);
            await _unitOfWork.CommitChangesAsync();
            var reloadEntity = await _fakeRepository.GetByIdAsync(id);

            Assert.Equal(entity.Name, name);
            Assert.Equal(entity, reloadEntity);
        }

        [Fact]
        public async Task TestGetById()
        {
            const string id = "A4C59043-DBEC-4A48-919C-3985809AB6F2";
            var entity = await _fakeRepository.GetByIdAsync(id);
            Assert.NotNull(entity);
            Assert.Equal(entity.Id, id);
        }

        #region soft-delete

        [Fact]
        public async Task TestSoftDelete()
        {
            const string id = "26D158E4-01C4-421B-8E72-E0999DB421FC";
            var entity = await _fakeRepository.GetByIdAsync(id);
            await _fakeRepository.DeleteAsync(entity);
            await _unitOfWork.CommitChangesAsync();
            var target = await _fakeRepository.GetByIdAsync(id);
            Assert.Null(target);
        }

        [Fact]
        public async Task TestGetSoftDeletedEntityInScope()
        {
            const string id = "E8231719-DFBF-472C-BA9F-3898CE7852FC";
            var entity = await _fakeRepository.GetByIdAsync(id);
            await _fakeRepository.DeleteAsync(entity);
            await _unitOfWork.CommitChangesAsync();

            var provider = ContainerBuilder.GetServiceProvider();
            using var scope = provider.GetRequiredService<IDataFilter>().Disable<ISoftDelete>();
            var target = await _fakeRepository.GetByIdAsync(id);
            Assert.NotNull(target);
            Assert.True(target.IsDeleted);
        }

        [Fact]
        public async Task TestGetSoftDeletedEntityOutScope()
        {
            const string id = "431DE9E8-AC1E-4ED1-9391-287D40953985";
            var entity = await _fakeRepository.GetByIdAsync(id);
            await _fakeRepository.DeleteAsync(entity);
            await _unitOfWork.CommitChangesAsync();

            var provider = ContainerBuilder.GetServiceProvider();
            using (var _ = provider.GetRequiredService<IDataFilter>().Disable<ISoftDelete>())
            {
            }

            var target = await _fakeRepository.GetByIdAsync(id);
            Assert.Null(target);
        }

        #endregion
    }
}