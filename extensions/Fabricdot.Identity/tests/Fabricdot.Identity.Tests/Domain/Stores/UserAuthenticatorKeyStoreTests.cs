using Fabricdot.Identity.Domain.Stores;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserAuthenticatorKeyStoreTests : UserStoreTestBase
{
    private readonly IUserAuthenticatorKeyStore<User> _userAuthenticatorKeyStore;

    public UserAuthenticatorKeyStoreTests()
    {
        _userAuthenticatorKeyStore = (IUserAuthenticatorKeyStore<User>)UserStore;
    }

    [InlineAutoData]
    [InlineData(null)]
    [Theory]
    public async Task SetAuthenticatorKeyAsync_GivenKey_Correctly(string key)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            await _userAuthenticatorKeyStore.SetAuthenticatorKeyAsync(user, key, default);

            user.Tokens.Should().ContainSingle(v => v.LoginProvider == UserStoreConstants.InternalLoginProvider
                                                    && v.Name == UserStoreConstants.AuthenticatorKeyTokenName
                                                    && v.Value == key);
        });
    }

    [AutoData]
    [Theory]
    public async Task GetAuthenticatorKeyAsync_Should_ReturnCorretly(string key)
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId);
            await _userAuthenticatorKeyStore.SetAuthenticatorKeyAsync(user, key, default);
            var actualKey = await _userAuthenticatorKeyStore.GetAuthenticatorKeyAsync(user, default);

            actualKey.Should().Be(key);
        });
    }
}