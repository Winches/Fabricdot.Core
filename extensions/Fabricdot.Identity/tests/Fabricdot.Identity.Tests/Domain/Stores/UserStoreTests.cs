using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserStoreTests : UserStoreTestBase
{
    [Fact]
    public async Task CreateAsync_GivenNull_ThrownException()
    {
        await UserStore.Awaiting(v => v.CreateAsync(null!, default)).Should().ThrowAsync<ArgumentNullException>();
    }

    [AutoData]
    [Theory]
    public async Task CreateAsync_GivenUser_Correctly(User user)
    {
        var res = await UserStore.CreateAsync(user, default);
        var retrievalUser = await UserRepository.GetByIdAsync(user.Id);

        res.Succeeded.Should().BeTrue();
        retrievalUser.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenNull_ThrownException()
    {
        await UserStore.Awaiting(v => v.UpdateAsync(null!, default)).Should().ThrowAsync<ArgumentNullException>();
    }

    [AutoData]
    [Theory]
    public async Task UpdateAsync_GivenUser_Correctly(string givenName)
    {
        var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
        user!.GivenName = givenName;
        var res = await UserStore.UpdateAsync(user, default);

        res.Succeeded.Should().BeTrue();
        user.GivenName.Should().Be(givenName);
    }

    [Fact]
    public async Task DeleteAsync_GivenNull_ThrownException()
    {
        await UserStore.Awaiting(v => v.DeleteAsync(null!, default)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_GivenUser_Correctly()
    {
        var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
        var res = await UserStore.DeleteAsync(user!, default);
        var retrievalUser = await UserRepository.GetByIdAsync(user!.Id);

        res.Succeeded.Should().BeTrue();
        retrievalUser.Should().BeNull();
    }

    [Fact]
    public async Task FindByIdAsync_GivenInput_ReturnCorrectly()
    {
        var userId = FakeDataBuilder.UserAndersId;
        var user = await UserStore.FindByIdAsync(userId.ToString(), default);

        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
        user.Roles.Should().NotBeEmpty();
    }

    [Fact]
    public async Task FindByNameAsync_GivenInput_ReturnCorrectly()
    {
        var normalizedUserName = LookupNormalizer.NormalizeName(FakeDataBuilder.UserAnders);
        var user = await UserStore.FindByNameAsync(normalizedUserName, default);

        user.Should().NotBeNull();
        user.NormalizedUserName.Should().Be(normalizedUserName);
        user.Roles.Should().NotBeEmpty();
    }
}
