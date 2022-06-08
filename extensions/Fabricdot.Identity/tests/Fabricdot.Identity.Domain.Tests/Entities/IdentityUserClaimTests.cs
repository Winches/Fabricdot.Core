using System;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserClaimTests
{
    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void Constructor_GivenInvalidClaimType_ThrowException(string claimType)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUserClaim(Guid.NewGuid(), claimType, null));
    }

    [Fact]
    public void Constructor_GivenNullClaimValue_Correctly()
    {
        _ = new IdentityUserClaim(Guid.NewGuid(), "claimType1", null);
    }

    [InlineData("claimType2", "value2")]
    [InlineData("claimType3", null)]
    [Theory]
    public void SetClaim_GivenInput_ChangeClaim(
        string claimType,
        string claimValue)
    {
        var claim = new IdentityUserClaim(Guid.NewGuid(), "claimType1", "value1");
        claim.SetClaim(claimType, claimValue);
        Assert.Equal(claimType, claim.ClaimType);
        Assert.Equal(claimValue, claim.ClaimValue);
    }

    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void SetClaim_GivenInvalidInput_ThrowException(string claimType)
    {
        Assert.ThrowsAny<Exception>(() => new IdentityUserClaim(Guid.NewGuid(), claimType, null));
    }
}