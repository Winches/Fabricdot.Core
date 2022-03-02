using System;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores
{
    public class RoleStoreTests : RoleStoreTestBase
    {
        [Fact]
        public async Task CreateAsync_GivenNull_ThrownException()
        {
            Task TestCode() => RoleStore.CreateAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task CreateAsync_GivenRole_Correctly()
        {
            var role = new Role(Guid.NewGuid(), "Administrator");
            var res = await RoleStore.CreateAsync(role, default);
            var retrievalRole = await RoleRepository.GetByIdAsync(role.Id);

            Assert.True(res.Succeeded);
            Assert.NotNull(retrievalRole);
        }

        [Fact]
        public async Task UpdateAsync_GivenNull_ThrownException()
        {
            Task TestCode() => RoleStore.UpdateAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task UpdateAsync_GivenRole_Correctly()
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            const string desc = "description1";
            role.Description = desc;
            var res = await RoleStore.UpdateAsync(role, default);

            Assert.True(res.Succeeded);
            Assert.Equal(desc, role.Description);
        }

        [Fact]
        public async Task DeleteAsync_GivenNull_ThrownException()
        {
            Task TestCode() => RoleStore.DeleteAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task DeleteAsync_GivenRole_Correctly()
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            var res = await RoleStore.DeleteAsync(role, default);
            var retrievalRole = await RoleRepository.GetByIdAsync(role.Id);

            Assert.True(res.Succeeded);
            Assert.Null(retrievalRole);
        }

        [Fact]
        public async Task FindByIdAsync_GivenInput_ReturnCorrectly()
        {
            var roleId = FakeDataBuilder.RoleAuthorId;
            var role = await RoleStore.FindByIdAsync(roleId.ToString(), default);

            Assert.NotNull(role);
            Assert.Equal(roleId, role.Id);
        }

        [Fact]
        public async Task FindByNameAsync_GivenInput_ReturnCorrectly()
        {
            var normalizedRoleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleAuthor);
            var role = await RoleStore.FindByNameAsync(normalizedRoleName, default);

            Assert.NotNull(role);
            Assert.Equal(normalizedRoleName, role.NormalizedName);
        }
    }
}