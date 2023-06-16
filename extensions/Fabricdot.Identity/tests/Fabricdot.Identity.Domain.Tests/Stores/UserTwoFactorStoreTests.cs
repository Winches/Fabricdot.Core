using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserTwoFactorStoreTests : UserStoreTestsBase
{
    [AutoData]
    [Theory]
    public async Task GetTwoFactorEnabledAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.TwoFactorEnabled;
        var actual = await Sut.GetTwoFactorEnabledAsync(user, default);

        actual.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task SetTwoFactorEnabledAsync_Should_Correctly(
        IdentityUser user,
        bool enabled)
    {
        await Sut.SetTwoFactorEnabledAsync(user, enabled, default);

        user.TwoFactorEnabled.Should().Be(enabled);
    }
}
