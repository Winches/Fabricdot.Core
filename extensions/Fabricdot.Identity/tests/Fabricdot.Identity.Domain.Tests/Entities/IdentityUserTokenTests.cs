using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserTokenTests : TestFor<IdentityUserToken>
{
    [InlineData("provider1", "")]
    [InlineData("provider2", null)]
    [InlineData("", "name1")]
    [InlineData(null, "name2")]
    [Theory]
    public void Constructor_GivenInvalidInput_ThrowException(
        string loginProvider,
        string tokeName)
    {
        Invoking(() => new IdentityUserToken(loginProvider, tokeName, null)).Should().Throw<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public void ChangeValue_GivenInput_CloneInstance(string value)
    {
        var newToken1 = Sut.ChangeValue(Sut.Value);
        var newToken2 = Sut.ChangeValue(value);

        Sut.Should()
           .Be(newToken1).And
           .NotBeSameAs(newToken1).And
           .NotBe(newToken2);
    }
}