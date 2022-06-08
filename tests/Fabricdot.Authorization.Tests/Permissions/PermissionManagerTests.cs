using System;
using System.Threading.Tasks;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionManagerTests
{
    protected IPermissionManager PermissionManager { get; } = new PermissionManager();

    [Fact]
    public async Task AddGroupAsync_GivenDuplicateGroup_Throw()
    {
        var group = new PermissionGroup("group1", "group 1");
        await PermissionManager.AddGroupAsync(group);

        await FluentActions.Awaiting(() => PermissionManager.AddGroupAsync(group))
                           .Should()
                           .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task AddGroupAsync_GivenDuplicatePermission_Throw()
    {
        const string name1 = "name1";
        var group1 = new PermissionGroup("group1", "group 1");
        group1.AddPermission(name1, nameof(name1));
        var group2 = new PermissionGroup("group2", "group 2");
        group2.AddPermission(name1, nameof(name1));
        await PermissionManager.AddGroupAsync(group1);

        await FluentActions.Awaiting(() => PermissionManager.AddGroupAsync(group2))
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AddGroupAsync_GivenInput_AddPermissions()
    {
        const string name1 = "name1";
        const string name2 = "name1.name2";
        var group = new PermissionGroup("group1", "group 1");
        group.AddPermission(name1, nameof(name1))
             .Add(name2, nameof(name2));

        await PermissionManager.AddGroupAsync(group);
        var permissionGroups = await PermissionManager.ListGroupsAsync();
        var permissions = await PermissionManager.ListAsync();

        permissionGroups.Should().Contain(group);
        permissions.Should().ContainSingle(v => v.Name == name1);
        permissions.Should().ContainSingle(v => v.Name == name2);
    }
}