using System.Net.Mail;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Stores;

public class UserEmailStoreTests : UserStoreTestsBase
{
    [AutoData]
    [Theory]
    public async Task GetEmailAsync_Should_ReturnCorrectly([Greedy] IdentityUser user)
    {
        var expected = user.Email;
        var email = await Sut.GetEmailAsync(user, default);

        email.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetNormalizedEmailAsync_Should_ReturnCorrectly([Greedy] IdentityUser user)
    {
        var expected = user.NormalizedEmail;
        var normalizedEmail = await Sut.GetNormalizedEmailAsync(user, default);

        normalizedEmail.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetEmailConfirmedAsync_Should_ReturnCorrectly(
        [Greedy] IdentityUser user,
        bool expected)
    {
        user.EmailConfirmed = expected;
        var emailConfirmed = await Sut.GetEmailConfirmedAsync(user, default);

        emailConfirmed.Should().Be(expected);
    }

    [InlineData(null)]
    [InlineAutoData]
    [Theory]
    public async Task SetEmailAsync_GiveInput_Correctly(MailAddress? email)
    {
        var expected = email?.Address;
        var user = Create<IdentityUser>();
        await Sut.SetEmailAsync(user, expected!, default);

        user.Email.Should().Be(expected);
    }

    [InlineData(null)]
    [InlineAutoData]
    [Theory]
    public async Task SetNormalizedEmailAsync_GiveInput_Correctly(MailAddress? email)
    {
        var expected = email?.Address?.Normalize()?.ToUpperInvariant();
        var user = Create<IdentityUser>();
        await Sut.SetNormalizedEmailAsync(user, expected!, default);

        user.NormalizedEmail.Should().Be(expected);
    }

    [InlineData(true)]
    [InlineData(false)]
    [Theory]
    public async Task SetEmailConfirmedAsync_GivenIpur_Correctly(bool expected)
    {
        var user = Create<IdentityUser>();
        await Sut.SetEmailConfirmedAsync(user, expected, default);

        user.EmailConfirmed.Should().Be(expected);
    }
}
