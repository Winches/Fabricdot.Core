using System.Threading.Tasks;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserSecurityStampStoreTests : UserStoreTestsBase
{
    [Fact]
    public async Task GetSecurityStampAsync_ReturnCorrectly()
    {
        var user = EntityBuilder.NewUser();
        var securityStamp = await UserStore.GetSecurityStampAsync(user, default);

        Assert.Equal(user.SecurityStamp, securityStamp);
    }

    [InlineData(null)]
    [InlineData("EC6B1C44595C4F5792E18E1BC0567876")]
    [Theory]
    public async Task SetSecurityStampAsync_GivenInput_Correctly(string securityStamp)
    {
        var user = EntityBuilder.NewUser();
        await UserStore.SetSecurityStampAsync(user, securityStamp, default);

        Assert.Equal(securityStamp, user.SecurityStamp);
    }
}