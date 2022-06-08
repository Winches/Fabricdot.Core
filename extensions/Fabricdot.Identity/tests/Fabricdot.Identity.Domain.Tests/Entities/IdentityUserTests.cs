using System;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserTests
{
    [Fact]
    public void Constructor_GivenUserName_TrimWhiteSpace()
    {
        const string userName = " name1 ";
        var user = new IdentityUser(Guid.NewGuid(), userName);
        Assert.Equal(userName.Trim(), user.UserName);
    }

    [Fact]
    public void Constructor_GivenUserName_NormalizeUserName()
    {
        const string userName = "name1";
        var user = new IdentityUser(Guid.NewGuid(), userName);
        Assert.Equal(userName.Normalize().ToUpperInvariant(), user.NormalizedUserName);
    }

    [Fact]
    public void Constructor_GivenEmail_TrimWhiteSpace()
    {
        const string email = " qwe@banana.com ";
        var user = new IdentityUser(Guid.NewGuid(), "name1", email);
        Assert.Equal(email.Trim(), user.Email);
    }

    [Fact]
    public void Constructor_GivenEmail_NormalizeEmail()
    {
        const string email = "qwe@banana.com";
        var user = new IdentityUser(Guid.NewGuid(), "name1", email);
        Assert.Equal(email.Normalize().ToUpperInvariant(), user.NormalizedEmail);
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidUserName_ThrowException(string userName)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUser(Guid.NewGuid(), userName));
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidEmail_ThrowException(string email)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUser(Guid.NewGuid(), "name1", email));
    }

    [Fact]
    public void Enable_UserIsActive()
    {
        var user = new IdentityUser(Guid.NewGuid(), "name1")
        {
            IsActive = false
        };
        user.Enable();
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Disable_UserIsInActive()
    {
        var user = new IdentityUser(Guid.NewGuid(), "name1")
        {
            IsActive = true
        };
        user.Disable();
        Assert.False(user.IsActive);
    }
}