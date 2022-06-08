using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserPasswordStoreTests : UserStoreTestsBase
{
    [Fact]
    public async Task GetPasswordHashAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUserWithPassword();
        var passwordHash = await UserStore.GetPasswordHashAsync(user, default);
        Assert.Equal(user.PasswordHash, passwordHash);
    }

    [InlineData(null)]
    [InlineData("PasswordHash")]
    [Theory]
    public async Task HasPasswordAsync_ReturnCorrectly(string passwordHash)
    {
        var user = EntityBuilder.NewUserWithPassword(passwordHash: passwordHash);
        var hasPassword = await UserStore.HasPasswordAsync(user, default);
        Assert.Equal(passwordHash != null, hasPassword);
    }

    [InlineData(null)]
    [InlineData("PasswordHash")]
    [Theory]
    public async Task SetPasswordHashAsync_GivenInput_Correctly(string passwordHash)
    {
        var user = EntityBuilder.NewUserWithPassword();
        await UserStore.SetPasswordHashAsync(user, passwordHash, default);
        Assert.Equal(passwordHash, user.PasswordHash);
    }
}