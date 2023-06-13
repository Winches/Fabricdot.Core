using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

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
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);

            await _userLoginStore.Awaiting(v => v.AddLoginAsync(user, null, default))
                                 .Should()
                                 .ThrowAsync<ArgumentNullException>();
        });
    }

    [AutoData]
    [Theory]
    public async Task AddLoginAsync_GivenLogin_Correctly(UserLoginInfo loginInfo)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            await _userLoginStore.AddLoginAsync(
                user,
                loginInfo,
                default);

            user.Logins.Should().ContainEquivalentOf(loginInfo, opts => opts.ExcludingMissingMembers());
        });
    }

    [Fact]
    public async Task RemoveLoginAsync_GivenLogin_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            var loginInfo = user.Logins.First();
            await _userLoginStore.RemoveLoginAsync(
                user,
                loginInfo.LoginProvider,
                loginInfo.ProviderKey,
                default);

            user.Logins.Should().NotContainEquivalentOf(loginInfo, opts => opts.ExcludingMissingMembers());
        });
    }

    [Fact]
    public async Task GetLoginsAsync_Should_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            var expected = user.Logins;
            var logins = await _userLoginStore.GetLoginsAsync(user, default);

            logins.Should().BeEquivalentTo(
                expected,
                opts => opts.ComparingByMembers<IdentityUserLogin>().ExcludingMissingMembers());
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

        user.Should().NotBeNull();
        user.Id.Should().Be(FakeDataBuilder.UserAndersId);
    }
}