using System.Security.Claims;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.SharedKernel;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityRoleClaimTests : TestBase
{
    [InlineData(null)]
    [InlineData("")]
    [Theory]
    public void Constructor_GivenInvalidClaimType_ThrowException(string? claimType)
    {
        Invoking(() => new IdentityRoleClaim(Guid.NewGuid(), claimType!, null)).Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_GivenNullClaimValue_Correctly()
    {
        _ = new IdentityRoleClaim(Create<Guid>(), Create<string>(), null);
    }

    [AutoMockData]
    [Theory]
    public void SetClaim_GivenInput_ChangeValue(IdentityRoleClaim roleClaim, Claim claim)
    {
        roleClaim.SetClaim(claim.Type, claim.Value);

        roleClaim.ToClaim().Should().BeEquivalentTo(claim);
    }
}
