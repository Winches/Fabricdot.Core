using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserClaimTests : TestFor<IdentityUserClaim>
{
    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void Constructor_GivenInvalidClaimType_ThrowException(string? claimType)
    {
        Invoking(() => new IdentityUserClaim(Guid.NewGuid(), claimType!, null)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_GivenNullClaimValue_Correctly()
    {
        _ = new IdentityUserClaim(Create<Guid>(), Create<string>(), null);
    }

    [AutoData]
    [Theory]
    public void SetClaim_GivenInput_ChangeClaim(
            string claimType,
            string claimValue)
    {
        var expected = new
        {
            claimType,
            claimValue
        };
        Sut.SetClaim(claimType, claimValue);

        Sut.Should().BeEquivalentTo(expected, opts => opts.ExcludingMissingMembers());
    }

    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void SetClaim_GivenInvalidInput_ThrowException(string? claimType)
    {
        Invoking(() => new IdentityUserClaim(Create<Guid>(), claimType!, Create<string>())).Should().Throw<ArgumentException>();
    }
}
