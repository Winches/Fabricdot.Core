using System.Linq;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionHandlerContextTests
{
    [Fact]
    public void Constructor_GivenInput_Correctly()
    {
        var subjects = new[] { new GrantSubject(GrantTypes.User, "1") };
        var permissions = new[] { new PermissionName("name1") };
        var context = new PermissionHandlerContext(subjects, permissions);

        context.Subjects.Should().BeEquivalentTo(subjects);
        context.Permissions.Should()
                           .BeEquivalentTo(permissions).And
                           .BeEquivalentTo(context.PendingPermissions);
    }

    [Fact]
    public void Succeed_Should_ClearPendingPermissions()
    {
        var subjects = new[] { new GrantSubject(GrantTypes.User, "1") };
        var permissions = new[] { new PermissionName("name1") };
        var context = new PermissionHandlerContext(subjects, permissions);
        context.Succeed();

        context.PendingPermissions.Should().BeEmpty();
    }

    [Fact]
    public void Succeed_GivenPermission_RemovePendingPermission()
    {
        var subjects = new[] { new GrantSubject(GrantTypes.User, "1") };
        var permissions = new[] { new PermissionName("name1") };
        var context = new PermissionHandlerContext(subjects, permissions);

        var permission = context.Permissions.First();
        context.Succeed(permission);
        context.PendingPermissions.Should().NotContain(permission);
    }

    [Fact]
    public void GetResults_Should_ReturnCorrectly()
    {
        var subjects = new[] { new GrantSubject(GrantTypes.User, "1") };
        var permissions = new[] { new PermissionName("name1") };
        var context = new PermissionHandlerContext(subjects, permissions);

        context.GetResults().Should().HaveCount(permissions.Length);
    }
}