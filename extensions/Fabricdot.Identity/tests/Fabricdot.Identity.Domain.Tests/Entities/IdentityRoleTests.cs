using System.Security.Claims;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityRoleTests : TestFor<IdentityRole>
{
    [AutoData]
    [Theory]
    public void Constructor_GivenName_TrimWhiteSpace(string roleName)
    {
        var role = new IdentityRole(Guid.NewGuid(), $" {roleName} ");

        role.Name.Should().Be(roleName);
    }

    [Fact]
    public void Constructor_GivenName_NormalizeName()
    {
        var expected = Sut.NormalizedName;

        Sut.Name.Normalize().ToUpperInvariant().Should().Be(expected);
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void Constructor_GivenInvalidName_ThrowException(string? roleName)
    {
        Invoking(() => new IdentityRole(Guid.NewGuid(), roleName!)).Should().Throw<ArgumentException>();
    }

    [AutoMockData]
    [Theory]
    public void AddClaim_GivenInput_Correctly(Claim claim)
    {
        Sut.AddClaim(Create<Guid>(), claim.Type, claim.Value);

        Sut.Claims.Should().ContainSingle(v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
    }

    [AutoData]
    [Theory]
    public void RemoveClaim_GivenInput_Correctly(string claimType, string[] claimValues)
    {
        var claimValue1 = claimValues[0];
        var claimValue2 = claimValues[^1];
        Sut.AddClaim(base.Create<Guid>(), claimType, claimValue1);
        Sut.AddClaim(Create<Guid>(), claimType, claimValue2);
        Sut.RemoveClaim(claimType, claimValue2);

        Sut.Claims.Should().Contain(v => v.ClaimType == claimType && v.ClaimValue == claimValue1);
        Sut.Claims.Should().NotContain(v => v.ClaimType == claimType && v.ClaimValue == claimValue2);
    }
}
