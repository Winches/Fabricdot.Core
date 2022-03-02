using System;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Xunit;

namespace Fabricdot.Identity.Domain.Tests.Entities
{
    public class IdentityRoleClaimTests
    {
        [InlineData(null)]
        [InlineData("")]
        [Theory]
        public void Constructor_GivenInvalidClaimType_ThrowException(string claimType)
        {
            Assert.ThrowsAny<Exception>(() => new IdentityRoleClaim(Guid.NewGuid(), claimType, null));
        }

        [Fact]
        public void Constructor_GivenNullClaimValue_Correctly()
        {
            _ = new IdentityRoleClaim(Guid.NewGuid(), "claimType1", null);
        }
    }
}