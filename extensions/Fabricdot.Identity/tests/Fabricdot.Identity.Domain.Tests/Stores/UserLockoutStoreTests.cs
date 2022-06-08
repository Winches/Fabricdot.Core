using System;
using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserLockoutStoreTests : UserStoreTestsBase
{
    [Fact]
    public async Task GetAccessFailedCountAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        user.AccessFailed();
        var accessFailedCount = await UserStore.GetAccessFailedCountAsync(user, default);

        Assert.Equal(user.AccessFailedCount, accessFailedCount);
    }

    [Fact]
    public async Task GetLockoutEnabledAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        var lockloutEnabled = await UserStore.GetLockoutEnabledAsync(user, default);

        Assert.Equal(user.LockoutEnabled, lockloutEnabled);
    }

    [InlineData("2099-01-01T00:00:00.0000000Z")]
    [InlineData("1970-01-01T00:00:00.0000000Z")]
    [Theory]
    public async Task GetLockoutEndDateAsync_ReturnCorrectly(string lockoutEndString)
    {
        var user = EntityBuilder.NewUser();
        user.Lockout(DateTime.Parse(lockoutEndString));
        var lockoutEnd = await UserStore.GetLockoutEndDateAsync(user, default);

        Assert.Equal(user.LockoutEnd, lockoutEnd);
    }

    [Fact]
    public async Task IncrementAccessFailedCountAsync_Correctly()
    {
        var user = EntityBuilder.NewUser();
        var accessFailedCount = user.AccessFailedCount;
        var failedCount = await UserStore.IncrementAccessFailedCountAsync(user, default);

        Assert.Equal(++accessFailedCount, user.AccessFailedCount);
        Assert.Equal(user.AccessFailedCount, failedCount);
    }

    [Fact]
    public async Task ResetAccessFailedCountAsync_Correctly()
    {
        var user = EntityBuilder.NewUser();
        await UserStore.IncrementAccessFailedCountAsync(user, default);
        await UserStore.ResetAccessFailedCountAsync(user, default);

        Assert.Equal(0, user.AccessFailedCount);
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task SetLockoutEnabledAsync_GivenInput_Correctly(bool lockoutEnabled)
    {
        var user = EntityBuilder.NewUser();
        await UserStore.SetLockoutEnabledAsync(user, lockoutEnabled, default);

        Assert.Equal(lockoutEnabled, user.LockoutEnabled);
    }

    [Fact]
    public async Task SetLockoutEndDateAsync_GivenInput_LockUser()
    {
        var user = EntityBuilder.NewUser();
        var lockoutEnd = DateTimeOffset.Now.AddMinutes(5);
        await UserStore.SetLockoutEndDateAsync(user, lockoutEnd, default);

        Assert.Equal(lockoutEnd, user.LockoutEnd);
        Assert.True(user.IsLockedOut);
    }

    [Fact]
    public async Task SetLockoutEndDateAsync_DisableLockout_DoNothing()
    {
        var user = EntityBuilder.NewUser(lockoutEnabled: false);
        var lockoutEnd = user.LockoutEnd;
        await UserStore.SetLockoutEndDateAsync(
            user,
            DateTimeOffset.Now.AddMinutes(5),
            default);

        Assert.Equal(lockoutEnd, user.LockoutEnd);
    }

    [Fact]
    public async Task SetLockoutEndDateAsync_GivenNull_UnlockUser()
    {
        var user = EntityBuilder.NewUser();
        user.Lockout(DateTimeOffset.Now.AddMinutes(5));
        await UserStore.SetLockoutEndDateAsync(user, null, default);

        Assert.Null(user.LockoutEnd);
        Assert.False(user.IsLockedOut);
    }
}