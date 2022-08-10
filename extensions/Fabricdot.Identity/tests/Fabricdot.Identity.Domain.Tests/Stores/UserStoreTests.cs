using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserStoreTests : UserStoreTestsBase
{
    [AutoData]
    [Theory]
    public async Task GetUserIdAsync_Shoud_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.Id.ToString();
        var id = await Sut.GetUserIdAsync(user, default);

        id.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetUserNameAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.UserName;
        var userName = await Sut.GetUserNameAsync(user, default);

        userName.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetNormalizedUserNameAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.NormalizedUserName;
        var normalizedUserName = await Sut.GetNormalizedUserNameAsync(user, default);

        normalizedUserName.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task SetUserNameAsync_GivenUserName_Correctly(
        IdentityUser user,
        string userName)
    {
        await Sut.SetUserNameAsync(user, userName, default);

        user.UserName.Should().Be(userName);
    }

    [Fact]
    public async Task SetUserNameAsync_GivenInvalidInput_ThrowException()
    {
        await Sut.Awaiting(v => v.SetUserNameAsync(null, Create<string>(), default)).Should().ThrowAsync<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public async Task SetNormalizedUserNameAsync_GivenNormalizedUserName_Correctly(
        IdentityUser user,
        string userName)
    {
        var expected = userName.Normalize().ToUpperInvariant();
        await Sut.SetNormalizedUserNameAsync(user, expected, default);

        user.NormalizedUserName.Should().Be(expected);
    }
}