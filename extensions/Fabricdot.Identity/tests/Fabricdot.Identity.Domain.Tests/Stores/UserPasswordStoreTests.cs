using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserPasswordStoreTests : UserStoreTestsBase
{
    public UserPasswordStoreTests()
    {
        Fixture.Customize<IdentityUser>(opts => opts.Do(v => v.PasswordHash = Create<string>()));
    }

    [Fact]
    public async Task GetPasswordHashAsync_Should_ReturnCorrectly()
    {
        var user = Create<IdentityUser>();
        var expected = user.PasswordHash;
        var passwordHash = await Sut.GetPasswordHashAsync(user, default);

        passwordHash.Should().Be(expected);
    }

    [InlineData(null)]
    [InlineAutoData]
    [Theory]
    public async Task HasPasswordAsync_Should_ReturnCorrectly(string passwordHash)
    {
        var user = Fixture.Build<IdentityUser>()
                  .WithAutoProperties()
                  .Do(v => v.PasswordHash = passwordHash)
                  .Create();
        var expected = passwordHash is not null;
        var hasPassword = await Sut.HasPasswordAsync(user, default);

        hasPassword.Should().Be(expected);
    }

    [InlineData(null)]
    [InlineAutoData]
    [Theory]
    public async Task SetPasswordHashAsync_GivenInput_Correctly(string passwordHash)
    {
        var user = Create<IdentityUser>();
        await Sut.SetPasswordHashAsync(user, passwordHash, default);

        user.PasswordHash.Should().Be(passwordHash);
    }
}
