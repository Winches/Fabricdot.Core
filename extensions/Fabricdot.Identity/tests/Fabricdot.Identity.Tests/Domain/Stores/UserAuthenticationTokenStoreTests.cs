using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserAuthenticationTokenStoreTests : UserStoreTestBase
{
    private readonly IUserAuthenticationTokenStore<User> _userAuthenticationTokenStore;

    public UserAuthenticationTokenStoreTests()
    {
        _userAuthenticationTokenStore = (IUserAuthenticationTokenStore<User>)UserStore;
    }

    [AutoData]
    [Theory]
    public async Task SetTokenAsync_GivenNewToken_AddToken(
        string loginProvider,
        string tokenName,
        string tokenValue)
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            await _userAuthenticationTokenStore.SetTokenAsync(
                user,
                loginProvider,
                tokenName,
                tokenValue,
                default);

            user.FindToken(loginProvider, tokenName).Should().NotBeNull();
        });
    }

    [AutoData]
    [Theory]
    public async Task SetTokenAsync_GivenExistedToken_UpdateToken(string tokenValue)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user!.Tokens.First();
            await _userAuthenticationTokenStore.SetTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                tokenValue,
                default);

            token = user.FindToken(token.LoginProvider, token.Name);

            token.Should().NotBeNull();
            token!.Value.Should().Be(tokenValue);
        });
    }

    [Fact]
    public async Task RemoveTokenAsync_GivenToken_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user!.Tokens.First();
            await _userAuthenticationTokenStore.RemoveTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                default);

            user.FindToken(token.LoginProvider, token.Name).Should().BeNull();
        });
    }

    [Fact]
    public async Task GetTokenAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user!.Tokens.First();
            var tokenValue = await _userAuthenticationTokenStore.GetTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                default);

            token.Value.Should().Be(tokenValue);
        });
    }
}
