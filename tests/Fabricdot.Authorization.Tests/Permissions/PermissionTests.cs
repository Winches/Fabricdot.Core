using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionTests : TestFor<Permission, PermissionCustomization>
{
    [Fact]
    public void Constructor_GivenInput_Correctly()
    {
        var permission1 = new Permission(Create<PermissionName>(), Create<string>(), null, null);
        var permission2 = new Permission(Create<PermissionName>(), Create<string>(), null, new[]
        {
            permission1
        });

        permission1.Children.Should().BeEmpty();
        permission2.Children.Should().ContainSingle(permission1);
    }

    [Fact]
    public void Constructor_GivenInvalidInput_Throw()
    {
        var sut = typeof(Permission).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Add_GivenInput_Correctly()
    {
        var expected = Create<Permission>();

        var self = Sut.Add(
            expected.Name,
            expected.DisplayName,
            expected.Description);

        Sut.Should().BeSameAs(self);
        Sut.Children.Should().ContainSingle(expected);
    }

    [AutoData]
    [Theory]
    public void Remove_GivenInput_Correctly(
        PermissionName name,
        string displayName)
    {
        Sut.Add(name, displayName);
        Sut.Remove(name);

        Sut.Children.Should().NotContain(v => v.Name == name);
    }

    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(Permission);
        var permission = new Permission(Sut.Name, Create<string>());

        Create<EqualityAssertion>().Verify(sut);
        Sut.Should().BeEquivalentTo(permission);
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var expected = $"Permission:{Sut.Name}";

        Sut.ToString().Should().Be(expected);
    }
}