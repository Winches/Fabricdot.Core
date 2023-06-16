using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserRoleStoreTests : UserStoreTestBase
{
    private readonly IUserRoleStore<User> _userRoleStore;

    public UserRoleStoreTests()
    {
        _userRoleStore = (IUserRoleStore<User>)UserStore;
    }

    [AutoData]
    [Theory]
    public async Task AddToRoleAsync_GivenNotExistsRole_ThrowException(string roleName)
    {
        var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

        await _userRoleStore.Awaiting(v => v.AddToRoleAsync(user!, roleName, default)).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AddToRoleAsync_GivenDuplicatedRole_DoNothing()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.RoleIdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleNameOfAnders);
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            user!.Roles.Should().ContainSingle(v => v.RoleId == roleId);
            await _userRoleStore.AddToRoleAsync(user, roleName, default);
            user.Roles.Should().ContainSingle(v => v.RoleId == roleId);
        });
    }

    [Fact]
    public async Task AddToRoleAsync_GivenRole_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.RoleAuthorId;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleAuthor);
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            user!.Roles.Should().NotContain(v => v.RoleId == roleId);
            await _userRoleStore.AddToRoleAsync(user, roleName, default);
            user.Roles.Should().ContainSingle(v => v.RoleId == roleId);
        });
    }

    [Fact]
    public async Task RemoveFromRoleAsync_GivenRole_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.RoleIdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleNameOfAnders);
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            user!.Roles.Should().ContainSingle(v => v.RoleId == roleId);
            await _userRoleStore.RemoveFromRoleAsync(user, roleName, default);
            user.Roles.Should().NotContain(v => v.RoleId == roleId);
        });
    }

    [AutoData]
    [Theory]
    public async Task RemoveFromRoleAsync_GivenUnincludedRole_DoNothing(string roleName)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            await _userRoleStore.Awaiting(v => v.RemoveFromRoleAsync(user!, roleName, default))
                                .Should()
                                .NotThrowAsync();
        });
    }

    [Fact]
    public async Task IsInRoleAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.RoleIdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleNameOfAnders);
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            user!.Roles.Should().ContainSingle(v => v.RoleId == roleId);
            (await _userRoleStore.IsInRoleAsync(user, roleName, default)).Should().BeTrue();
            (await _userRoleStore.IsInRoleAsync(user, FakeDataBuilder.RoleAuthor, default)).Should().BeFalse();
            (await _userRoleStore.IsInRoleAsync(user, Create<string>(), default)).Should().BeFalse();
        });
    }

    [Fact]
    public async Task GetRolesAsync_ReturnCorrectly()
    {
        var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
        var roleNames = await _userRoleStore.GetRolesAsync(user!, default);

        roleNames.Should().Contain(FakeDataBuilder.RoleNameOfAnders);
    }

    [Fact]
    public async Task GetUsersInRoleAsync_ReturnCorrectly()
    {
        var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleNameOfAnders);
        var users = await _userRoleStore.GetUsersInRoleAsync(roleName, default);

        users.Should().Contain(v => v.Id == FakeDataBuilder.UserAndersId);
    }
}
