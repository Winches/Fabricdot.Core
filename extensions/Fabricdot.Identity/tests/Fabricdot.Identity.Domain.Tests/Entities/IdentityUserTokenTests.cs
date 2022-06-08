using System;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserTokenTests
{
    [InlineData("provider1", "")]
    [InlineData("provider2", null)]
    [InlineData("", "name1")]
    [InlineData(null, "name2")]
    [Theory]
    public void Constructor_GivenInvalidInput_ThrowException(
        string loginProvider,
        string tokeName)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUserToken(loginProvider, tokeName, null));
    }

    [Fact]
    public void ChangeValue_GivenInput_CloneInstance()
    {
        var token = new IdentityUserToken("provider1", "name1", "value1");
        var newToken1 = token.ChangeValue(token.Value);
        var newToken2 = token.ChangeValue("value2");

        Assert.Equal(token, newToken1);
        Assert.NotEqual(token, newToken2);
        Assert.NotSame(token, newToken1);
    }
}