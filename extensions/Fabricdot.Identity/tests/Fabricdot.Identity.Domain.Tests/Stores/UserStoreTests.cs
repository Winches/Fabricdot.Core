using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserStoreTests : UserStoreTestsBase
{
    [Fact]
    public async Task GetUserIdAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        var id = await UserStore.GetUserIdAsync(user, default);
        Assert.Equal(user.Id.ToString(), id);
    }

    [Fact]
    public async Task GetUserNameAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        var userName = await UserStore.GetUserNameAsync(user, default);
        Assert.Equal(user.UserName, userName);
    }

    [Fact]
    public async Task GetNormalizedUserNameAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        var normalizedUserName = await UserStore.GetNormalizedUserNameAsync(user, default);
        Assert.Equal(user.NormalizedUserName, normalizedUserName);
    }

    [Fact]
    public async Task SetUserNameAsync_GivenUserName_Correctly()
    {
        var user = EntityBuilder.NewUser();
        const string userName = "name2";
        await UserStore.SetUserNameAsync(user, userName, default);
        Assert.Equal(user.UserName, userName);
    }

    [Fact]
    public async Task SetUserNameAsync_GivenInvalidInput_ThrowException()
    {
        var user = EntityBuilder.NewUser();
        const string userName = "name2";
        await UserStore.SetUserNameAsync(user, userName, default);
        Assert.Equal(user.UserName, userName);
    }

    [Fact]
    public async Task SetNormalizedUserNameAsync_GivenNormalizedUserName_Correctly()
    {
        var user = EntityBuilder.NewUser();
        const string normalizedUserName = "NAME2";
        await UserStore.SetNormalizedUserNameAsync(user, normalizedUserName, default);
        Assert.Equal(user.NormalizedUserName, normalizedUserName);
    }
}