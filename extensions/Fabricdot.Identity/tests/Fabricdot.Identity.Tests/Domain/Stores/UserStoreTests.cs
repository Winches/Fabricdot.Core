using System;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores
{
    public class UserStoreTests : UserStoreTestBase
    {
        [Fact]
        public async Task CreateAsync_GivenNull_ThrownException()
        {
            Task TestCode() => UserStore.CreateAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task CreateAsync_GivenUser_Correctly()
        {
            var user = new User(Guid.NewGuid(), "James");
            var res = await UserStore.CreateAsync(user, default);
            var retrievalUser = await UserRepository.GetByIdAsync(user.Id);

            Assert.True(res.Succeeded);
            Assert.NotNull(retrievalUser);
        }

        [Fact]
        public async Task UpdateAsync_GivenNull_ThrownException()
        {
            Task TestCode() => UserStore.UpdateAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task UpdateAsync_GivenUser_Correctly()
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            const string givenName = "Anders1";
            user.GivenName = givenName;
            var res = await UserStore.UpdateAsync(user, default);

            Assert.True(res.Succeeded);
            Assert.Equal(givenName, user.GivenName);
        }

        [Fact]
        public async Task DeleteAsync_GivenNull_ThrownException()
        {
            Task TestCode() => UserStore.DeleteAsync(null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        }

        [Fact]
        public async Task DeleteAsync_GivenUser_Correctly()
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var res = await UserStore.DeleteAsync(user, default);
            var retrievalUser = await UserRepository.GetByIdAsync(user.Id);

            Assert.True(res.Succeeded);
            Assert.Null(retrievalUser);
        }

        [Fact]
        public async Task FindByIdAsync_GivenInput_ReturnCorrectly()
        {
            var userId = FakeDataBuilder.UserAndersId;
            var user = await UserStore.FindByIdAsync(userId.ToString(), default);

            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            Assert.NotEmpty(user.Roles);
        }

        [Fact]
        public async Task FindByNameAsync_GivenInput_ReturnCorrectly()
        {
            var normalizedUserName = LookupNormalizer.NormalizeName(FakeDataBuilder.UserAnders);
            var user = await UserStore.FindByNameAsync(normalizedUserName, default);

            Assert.NotNull(user);
            Assert.Equal(normalizedUserName, user.NormalizedUserName);
            Assert.NotEmpty(user.Roles);
        }
    }
}