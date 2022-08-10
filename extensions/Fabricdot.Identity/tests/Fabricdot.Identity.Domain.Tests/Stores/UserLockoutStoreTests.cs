using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserLockoutStoreTests : UserStoreTestsBase
{
    [AutoData]
    [Theory]
    public async Task GetAccessFailedCountAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        user.AccessFailed();
        var expected = user.AccessFailedCount;
        var accessFailedCount = await Sut.GetAccessFailedCountAsync(user, default);

        accessFailedCount.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetLockoutEnabledAsync_Should_ReturnCorrectly(IdentityUser user)
    {
        var expected = user.LockoutEnabled;
        var lockloutEnabled = await Sut.GetLockoutEnabledAsync(user, default);

        lockloutEnabled.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetLockoutEndDateAsync_Should_ReturnCorrectly(
        IdentityUser user,
        DateTime lockoutEndTime)
    {
        user.LockoutEnabled = true;
        user.Lockout(lockoutEndTime);
        var expected = user.LockoutEnd;
        var lockoutEnd = await Sut.GetLockoutEndDateAsync(user, default);

        lockoutEnd.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task IncrementAccessFailedCountAsync_Should_Correctly(IdentityUser user)
    {
        var accessFailedCount = user.AccessFailedCount;
        var failedCount = await Sut.IncrementAccessFailedCountAsync(user, default);

        failedCount.Should()
                   .Be(user.AccessFailedCount).And
                   .Be(++accessFailedCount);
    }

    [AutoData]
    [Theory]
    public async Task ResetAccessFailedCountAsync_Should_Correctly(IdentityUser user)
    {
        await Sut.IncrementAccessFailedCountAsync(user, default);
        await Sut.ResetAccessFailedCountAsync(user, default);

        user.AccessFailedCount.Should().Be(0);
    }

    [AutoData]
    [Theory]
    public async Task SetLockoutEnabledAsync_GivenInput_Correctly(
        IdentityUser user,
        bool lockoutEnabled)
    {
        await Sut.SetLockoutEnabledAsync(user, lockoutEnabled, default);

        user.LockoutEnabled.Should().Be(lockoutEnabled);
    }

    [AutoData]
    [Theory]
    public async Task SetLockoutEndDateAsync_GivenInput_LockUser(
        IdentityUser user,
        DateTimeOffset lockoutEnd)
    {
        var expected = lockoutEnd > DateTimeOffset.UtcNow;
        user.LockoutEnabled = true;
        await Sut.SetLockoutEndDateAsync(user, lockoutEnd, default);

        user.LockoutEnd.Should().Be(lockoutEnd);
        user.IsLockedOut.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task SetLockoutEndDateAsync_DisableLockout_DoNothing(IdentityUser user)
    {
        user.LockoutEnabled = false;
        var lockoutEnd = user.LockoutEnd;
        await Sut.SetLockoutEndDateAsync(
            user,
            DateTimeOffset.Now.AddMinutes(5),
            default);

        user.LockoutEnd.Should().Be(lockoutEnd);
    }

    [AutoData]
    [Theory]
    public async Task SetLockoutEndDateAsync_GivenNull_UnlockUser(
        IdentityUser user,
        DateTimeOffset lockoutEnd)
    {
        user.LockoutEnabled = true;
        user.Lockout(lockoutEnd);
        await Sut.SetLockoutEndDateAsync(user, null, default);

        user.LockoutEnd.Should().BeNull();
        user.IsLockedOut.Should().BeFalse();
    }
}