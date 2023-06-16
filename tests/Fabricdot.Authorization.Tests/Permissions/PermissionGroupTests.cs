using AutoFixture.Idioms;
using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionGroupTests : TestFor<PermissionGroup, PermissionCustomization>
{
    [Fact]
    public void Constructor_GivenInvalidInput_Throw()
    {
        var sut = typeof(PermissionGroup).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void Constructor_GivenInput_InitializeMember()
    {
        var sut = typeof(PermissionGroup).GetConstructors();

        Create<ConstructorInitializedMemberAssertion>().Verify(sut);
    }


    [Fact]
    public void AddPermission_GivenInput_Correctly()
    {
        var expected = Create<Permission>();
        var permission = Sut.AddPermission(
            expected.Name,
            expected.DisplayName,
            expected.Description);

        permission.Should().BeEquivalentTo(expected);
        Sut.Permissions.Should().Contain(permission);
    }

    [Fact]
    public void AddPermission_GivenDuplicateValue_Throw()
    {
        var permission = Create<Permission>();
        void TestCode() => Sut.AddPermission(permission.Name, permission.DisplayName);
        TestCode();

        Invoking(TestCode)
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    [AutoData]
    [Theory]
    public void RemovePermission_GivenInput_Correctly(
        PermissionName name,
        string displayName)
    {
        var permission = Sut.AddPermission(name, displayName);
        Sut.RemovePermission(permission);

        Sut.Permissions.Should().NotContain(permission);
    }

    [Fact]
    public void ToString_Should_ReturnCorrectly()
    {
        var expected = $"PermissionGroup:{Sut.Name}";

        Sut.ToString().Should().Be(expected);
    }
}
