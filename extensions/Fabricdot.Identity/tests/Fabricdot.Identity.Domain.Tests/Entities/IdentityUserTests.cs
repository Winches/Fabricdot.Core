using AutoFixture.Kernel;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserTests : TestFor<IdentityUser>
{
    public IdentityUserTests()
    {
        Fixture.Customize<IdentityUser>(v => v.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }

    [Fact]
    public void Constructor_GivenUserName_TrimWhiteSpace()
    {
        var expected = Sut.UserName;

        Sut.UserName.Trim().Should().Be(expected);
    }

    [Fact]
    public void Constructor_GivenUserName_NormalizeUserName()
    {
        var expected = Sut.NormalizedUserName;

        Sut.UserName.Normalize().ToUpperInvariant().Should().Be(expected);
    }

    [Fact]
    public void Constructor_GivenEmail_TrimWhiteSpace()
    {
        var expected = Sut.Email;

        Sut.Email.Trim().Should().Be(expected);
    }

    [Fact]
    public void Constructor_GivenEmail_NormalizeEmail()
    {
        var expected = Sut.NormalizedEmail;

        Sut.Email.Normalize().ToUpperInvariant().Should().Be(expected);
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidUserName_ThrowException(string userName)
    {
        Invoking(() => new IdentityUser(Create<Guid>(), userName)).Should().Throw<ArgumentException>();
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidEmail_ThrowException(string email)
    {
        Invoking(() => new IdentityUser(Create<Guid>(), Create<string>(), email)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Enable_Should_Active()
    {
        Sut.Enable();

        Sut.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Disable_Should_InActive()
    {
        Sut.Disable();

        Sut.IsActive.Should().BeFalse();
    }

    [Fact]
    public void ClearRoles_Should_Correctly()
    {
        Sut.AddRole(Create<Guid>());
        Sut.ClearRoles();

        Sut.Roles.Should().BeEmpty();
    }
}
