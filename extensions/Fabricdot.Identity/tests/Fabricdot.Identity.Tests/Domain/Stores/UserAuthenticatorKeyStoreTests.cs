using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Identity.Domain.Stores;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserAuthenticatorKeyStoreTests : UserStoreTestBase
{
    private readonly IUserAuthenticatorKeyStore<User> _userAuthenticatorKeyStore;

    public UserAuthenticatorKeyStoreTests()
    {
        _userAuthenticatorKeyStore = (IUserAuthenticatorKeyStore<User>)UserStore;
    }

    [InlineData("key1")]
    [InlineData(null)]
    [Theory]
    public async Task SetAuthenticatorKeyAsync_GivenKey_Correctly(string key)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var actualKey = await SetAuthenticatorKeyAsync(user, key);

            Assert.Equal(key, actualKey);
        });
    }

    [Fact]
    public async Task GetAuthenticatorKeyAsync_ReturnCorretly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var key = await SetAuthenticatorKeyAsync(user, "key1");
            var actualKey = await _userAuthenticatorKeyStore.GetAuthenticatorKeyAsync(user, default);

            Assert.Equal(actualKey, key);
        });
    }

    private async Task<string> SetAuthenticatorKeyAsync(
        User user,
        string key)
    {
        await _userAuthenticatorKeyStore.SetAuthenticatorKeyAsync(user, key, default);
        var token = user.Tokens.SingleOrDefault(v => v.LoginProvider == UserStoreConstants.InternalLoginProvider
                                                     && v.Name == UserStoreConstants.AuthenticatorKeyTokenName);
        return token?.Value;
    }
}