using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserPhoneNumberStoreTests : UserStoreTestsBase
{
    public UserPhoneNumberStoreTests()
    {
        Fixture.Customize<IdentityUser>(opts => opts.Do(v => v.ChangePhoneNumber(Create<string>(), Create<bool>())));
    }

    [Fact]
    public async Task GetPhoneNumberAsync_Should_ReturnCorrectly()
    {
        var user = Create<IdentityUser>();
        var expected = user.PhoneNumber;
        var phoneNumber = await Sut.GetPhoneNumberAsync(user, default);

        phoneNumber.Should().Be(expected);
    }

    [Fact]
    public async Task GetPhoneNumberConfirmedAsync_Should_ReturnCorrectly()
    {
        var user = Create<IdentityUser>();
        var expected = user.PhoneNumberConfirmed;
        var phoneNumberConfirmed = await Sut.GetPhoneNumberConfirmedAsync(user, default);

        phoneNumberConfirmed.Should().Be(expected);
    }

    [InlineData(null)]
    [InlineAutoData]
    [Theory]
    public async Task SetPhoneNumberAsync_GivenInput_Correctly(string? phoneNumber)
    {
        var user = Create<IdentityUser>();
        var confirmed = user.PhoneNumberConfirmed;
        await Sut.SetPhoneNumberAsync(user, phoneNumber is null ? null! : $" {phoneNumber} ", default);

        user.PhoneNumber.Should().Be(phoneNumber);
        user.PhoneNumberConfirmed.Should().Be(confirmed);
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task SetPhoneNumberConfirmedAsync_GivenInput_Correctly(bool isConfirmed)
    {
        var user = Create<IdentityUser>();
        await Sut.SetPhoneNumberConfirmedAsync(user, isConfirmed, default);

        user.PhoneNumberConfirmed.Should().Be(isConfirmed);
    }
}
