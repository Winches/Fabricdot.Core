using System;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserRoleStoreTests : UserStoreTestBase
{
    private readonly IUserRoleStore<User> _userRoleStore;

    public UserRoleStoreTests()
    {
        _userRoleStore = (IUserRoleStore<User>)UserStore;
    }

    [Fact]
    public async Task AddToRoleAsync_GivenNotExistsRole_ThrowException()
    {
        var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
        Task testCode() => _userRoleStore.AddToRoleAsync(user, "RoleNotExist", default);

        await Assert.ThrowsAsync<InvalidOperationException>(testCode);
    }

    [Fact]
    public async Task AddToRoleAsync_GivenDuplicatedRole_DoNothing()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.Role1IdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.Role1NameOfAnders);
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);

            Assert.Contains(user.Roles, v => v.RoleId == roleId);
            await _userRoleStore.AddToRoleAsync(user, roleName, default);
            Assert.Single(user.Roles, v => v.RoleId == roleId);
        });
    }

    [Fact]
    public async Task AddToRoleAsync_GivenRole_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.RoleAuthorId;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleAuthor);
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);

            Assert.DoesNotContain(user.Roles, v => v.RoleId == roleId);
            await _userRoleStore.AddToRoleAsync(user, roleName, default);
            Assert.Single(user.Roles, v => v.RoleId == roleId);
        });
    }

    [Fact]
    public async Task RemoveFromRoleAsync_GivenRole_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.Role1IdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.Role1NameOfAnders);
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);

            Assert.Contains(user.Roles, v => v.RoleId == roleId);
            await _userRoleStore.RemoveFromRoleAsync(user, roleName, default);
            Assert.DoesNotContain(user.Roles, v => v.RoleId == roleId);
        });
    }

    [Fact]
    public async Task RemoveFromRoleAsync_GivenUnincludedRole_DoNothing()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            await _userRoleStore.RemoveFromRoleAsync(user, "RoleNotExist", default);
        });
    }

    [Fact]
    public async Task IsInRoleAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var roleId = FakeDataBuilder.Role1IdOfAnders;
            var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.Role1NameOfAnders);
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);

            Assert.Contains(user.Roles, v => v.RoleId == FakeDataBuilder.Role1IdOfAnders);
            Assert.True(await _userRoleStore.IsInRoleAsync(user, roleName, default));
            Assert.False(await _userRoleStore.IsInRoleAsync(user, FakeDataBuilder.RoleAuthor, default));
            Assert.False(await _userRoleStore.IsInRoleAsync(user, "RoleNotExist", default));
        });
    }

    [Fact]
    public async Task GetRolesAsync_ReturnCorrectly()
    {
        var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
        var roleNames = await _userRoleStore.GetRolesAsync(user, default);
        Assert.Contains(FakeDataBuilder.Role1NameOfAnders, roleNames);
    }

    [Fact]
    public async Task GetUsersInRoleAsync_ReturnCorrectly()
    {
        var roleName = LookupNormalizer.NormalizeName(FakeDataBuilder.Role1NameOfAnders);
        var users = await _userRoleStore.GetUsersInRoleAsync(roleName, default);
        Assert.Contains(users, v => v.Id == FakeDataBuilder.UserAndersId);
    }
}