using System;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserAuthenticationTokenStoreTests : UserStoreTestBase
{
    private readonly IUserAuthenticationTokenStore<User> _userAuthenticationTokenStore;

    public UserAuthenticationTokenStoreTests()
    {
        _userAuthenticationTokenStore = (IUserAuthenticationTokenStore<User>)UserStore;
    }

    [Fact]
    public async Task SetTokenAsync_GivenNewToken_AddToken()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            const string loginProvider = "NewProvider";
            const string tokenName = "name1";
            await _userAuthenticationTokenStore.SetTokenAsync(
                user,
                loginProvider,
                tokenName,
                "1",
                default);

            Assert.NotNull(user.FindToken(loginProvider, tokenName));
        });
    }

    [Fact]
    public async Task SetTokenAsync_GivenExistedToken_UpdateToken()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user.Tokens.First();
            var tokenValue = Guid.NewGuid().ToString("N");
            await _userAuthenticationTokenStore.SetTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                tokenValue,
                default);
            token = user.FindToken(token.LoginProvider, token.Name);

            Assert.NotNull(token);
            Assert.Equal(tokenValue, token.Value);
        });
    }

    [Fact]
    public async Task RemoveTokenAsync_GivenToken_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user.Tokens.First();
            await _userAuthenticationTokenStore.RemoveTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                default);
            var removedToken = user.FindToken(token.LoginProvider, token.Name);

            Assert.Null(removedToken);
        });
    }

    [Fact]
    public async Task GetTokenAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var token = user.Tokens.First();
            var tokenValue = await _userAuthenticationTokenStore.GetTokenAsync(
                user,
                token.LoginProvider,
                token.Name,
                default);

            Assert.Equal(token.Value, tokenValue);
        });
    }
}