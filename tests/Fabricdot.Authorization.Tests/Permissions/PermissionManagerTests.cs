using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionManagerTests : TestFor<PermissionManager>
{
    [AutoData]
    [Theory]
    public async Task AddGroupAsync_GivenDuplicateGroup_Throw(PermissionGroup group)
    {
        Task TestCode() => Sut.AddGroupAsync(group);
        await TestCode();

        await Awaiting(TestCode)
                           .Should()
                           .ThrowAsync<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public async Task AddGroupAsync_GivenDuplicatePermission_Throw(
        PermissionGroup group1,
        PermissionGroup group2,
        string permission)
    {
        group1.AddPermission(permission, nameof(permission));
        group2.AddPermission(permission, nameof(permission));
        await Sut.AddGroupAsync(group1);

        await Awaiting(() => Sut.AddGroupAsync(group2))
                           .Should()
                           .ThrowAsync<InvalidOperationException>();
    }

    [AutoData]
    [Theory]
    public async Task AddGroupAsync_GivenInput_AddPermissions(
        string permission1,
        string permission2,
        PermissionGroup group)
    {
        group.AddPermission(permission1, nameof(permission1))
             .Add(permission2, nameof(permission2));

        await Sut.AddGroupAsync(group);
        var permissionGroups = await Sut.ListGroupsAsync();
        var permissions = await Sut.ListAsync();

        permissionGroups.Should().Contain(group);
        permissions.Should().ContainSingle(v => v.Name == permission1);
        permissions.Should().ContainSingle(v => v.Name == permission2);
    }
}
