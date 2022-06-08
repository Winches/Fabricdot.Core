using System;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserLoginTests
{
    [InlineData("provider1", "")]
    [InlineData("provider2", null)]
    [InlineData("", "key1")]
    [InlineData(null, "key2")]
    [Theory]
    public void Constructor_GivenInvalidInput_ThrowException(
        string loginProvider,
        string providerKey)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUserLogin(loginProvider, providerKey, null));
    }
}