using System;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserLoginStoreTests : UserStoreTestBase
{
    private readonly IUserLoginStore<User> _userLoginStore;

    public UserLoginStoreTests()
    {
        _userLoginStore = (IUserLoginStore<User>)UserStore;
    }

    [Fact]
    public async Task AddLoginAsync_GivenNull_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            Task TestCode() => _userLoginStore.AddLoginAsync(user, null, default);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        });
    }

    [Fact]
    public async Task AddLoginAsync_GivenLogin_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var login = new UserLoginInfo("provider1", "key1", "name1");
            await _userLoginStore.AddLoginAsync(
                user,
                login,
                default);
            Assert.Contains(user.Logins, v => v.LoginProvider == login.LoginProvider && v.ProviderKey == login.ProviderKey);
        });
    }

    [Fact]
    public async Task RemoveLoginAsync_GivenLogin_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var login = user.Logins.First();
            await _userLoginStore.RemoveLoginAsync(
                user,
                login.LoginProvider,
                login.ProviderKey,
                default);

            Assert.DoesNotContain(user.Logins, v => v.LoginProvider == login.LoginProvider && v.ProviderKey == login.ProviderKey);
        });
    }

    [Fact]
    public async Task GetLoginsAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var logins = await _userLoginStore.GetLoginsAsync(user, default);
            Assert.Contains(logins, v => user.Logins.Any(o => o.LoginProvider == v.LoginProvider && o.ProviderKey == v.ProviderKey));
        });
    }

    [Fact]
    public async Task FindByLoginAsync_ReturnCorrectly()
    {
        var login = FakeDataBuilder.LoginOfAnders;
        var user = await _userLoginStore.FindByLoginAsync(
            login.LoginProvider,
            login.ProviderKey,
            default);

        Assert.NotNull(user);
        Assert.Equal(FakeDataBuilder.UserAndersId, user.Id);
    }
}