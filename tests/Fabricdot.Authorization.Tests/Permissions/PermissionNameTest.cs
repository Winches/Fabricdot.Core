using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionNameTest : TestFor<PermissionName>
{
    [Fact]
    public void Constructor_GivenInvalidInput_Throw()
    {
        var sut = typeof(PermissionName).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void EqualsOperator_Should_ReturnCorrectly()
    {
        var name = new PermissionName(Sut.Value);

        (Sut != name).Should().BeFalse();
        (Sut == name).Should().BeTrue();
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(PermissionName);

        Create<EqualityAssertion>().Verify(sut);
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var expected = Sut.Value;

        Sut.ToString().Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public void ConversionOperator_Should_ReturnCorrectly(string permission)
    {
        string _ = (PermissionName)permission;
    }
}
