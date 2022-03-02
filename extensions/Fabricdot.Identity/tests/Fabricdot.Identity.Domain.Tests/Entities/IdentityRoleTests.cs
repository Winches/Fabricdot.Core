using System;
using System.Linq;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities
{
    public class IdentityRoleTests
    {
        [Fact]
        public void Constructor_GivenName_TrimWhiteSpace()
        {
            const string roleName = " Administrator ";
            var role = new IdentityRole(Guid.NewGuid(), roleName);
            Assert.Equal(roleName.Trim(), role.Name);
        }

        [Fact]
        public void Constructor_GivenName_NormalizeName()
        {
            const string roleName = "Administrator";
            var role = new IdentityRole(Guid.NewGuid(), roleName);
            Assert.Equal(roleName.Normalize().ToUpperInvariant(), role.NormalizedName);
        }

        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [Theory]
        public void Constructor_GivenInvalidName_ThrowException(string roleName)
        {
            Assert.ThrowsAny<Exception>(() => new IdentityRole(Guid.NewGuid(), roleName));
        }

        [Fact]
        public void AddClaim_GivenInput_Correctly()
        {
            var role = new IdentityRole(Guid.NewGuid(), "Administrator");
            const string claimType = "claimType1";
            const string claimValue = "claimValue1";
            role.AddClaim(Guid.NewGuid(), claimType, claimValue);
            var claim = role.Claims.Single();

            Assert.Equal(claimType, claim.ClaimType);
            Assert.Equal(claimValue, claim.ClaimValue);
        }

        [Fact]
        public void RemoveClaim_GivenInput_Correctly()
        {
            var role = new IdentityRole(Guid.NewGuid(), "Administrator");
            const string claimType = "claimType1";
            const string claimValue1 = "claimValue1";
            const string claimValue2 = "claimValue2";
            role.AddClaim(Guid.NewGuid(), claimType, claimValue1);
            role.AddClaim(Guid.NewGuid(), claimType, claimValue2);
            role.RemoveClaim(claimType, claimValue1);

            Assert.DoesNotContain(role.Claims, v => v.ClaimType == claimType && v.ClaimValue == claimValue1);
            Assert.Contains(role.Claims, v => v.ClaimType == claimType && v.ClaimValue == claimValue2);
        }
    }
}