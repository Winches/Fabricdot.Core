using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionHandlerContextTests : TestFor<PermissionHandlerContext>
{
    [AutoData]
    [Theory]
    public void Constructor_GivenInput_Correctly(
        GrantSubject[] subjects,
        PermissionName[] permissions)
    {
        var context = new PermissionHandlerContext(subjects, permissions);

        context.Subjects.Should().BeEquivalentTo(subjects);
        context.Permissions.Should()
                           .BeEquivalentTo(permissions).And
                           .BeEquivalentTo(context.PendingPermissions);
    }

    [Fact]
    public void Succeed_Should_ClearPendingPermissions()
    {
        Sut.Succeed();

        Sut.PendingPermissions.Should().BeEmpty();
    }

    [Fact]
    public void Succeed_GivenPermission_RemovePendingPermission()
    {
        var permission = Sut.Permissions.First();
        Sut.Succeed(permission);

        Sut.PendingPermissions.Should().NotContain(permission);
    }

    [Fact]
    public void GetResults_Should_ReturnCorrectly()
    {
        var expected = Sut.Permissions;

        Sut.GetResults().Should().HaveSameCount(expected);
    }
}
