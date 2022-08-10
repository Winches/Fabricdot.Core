using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserSecurityStampStoreTests : UserStoreTestsBase
{
    [AutoData]
    [Theory]
    public async Task GetSecurityStampAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.SecurityStamp;
        var securityStamp = await Sut.GetSecurityStampAsync(user, default);

        securityStamp.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task SetSecurityStampAsync_Should_GivenInput_Correctly(
        IdentityUser user,
        string securityStamp)
    {
        await Sut.SetSecurityStampAsync(user, securityStamp, default);

        user.SecurityStamp.Should().Be(securityStamp);
    }
}
