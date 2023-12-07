using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Testing.AutoFixture;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserLoginTests : TestBase
{
    [InlineData("provider1", "")]
    [InlineData("provider2", null)]
    [InlineData("", "key1")]
    [InlineData(null, "key2")]
    [Theory]
    public void Constructor_GivenInvalidInput_ThrowException(
        string? loginProvider,
        string? providerKey)
    {
        Invoking(() => new IdentityUserLogin(loginProvider!, providerKey!, null)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(IdentityUserLogin);

        Create<EqualityAssertion>().Verify(sut);
    }
}
