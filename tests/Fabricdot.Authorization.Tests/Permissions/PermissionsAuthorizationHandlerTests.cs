using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions
{
    public class PermissionsAuthorizationHandlerTests : AuthrizationHandlerTestsBase<PermissionsAuthorizationHandler>
    {
        public static IEnumerable<object[]> GetRequirements()
        {
            var mixedPermissions = UngrantedPermissions.Union(GrantedPermissions).ToArray();

            yield return new object[]
            {
                new[] { new PermissionsRequirement(UngrantedPermissions, PermissionRequireBehavior.Any) },
                false
            };

            yield return new object[]
            {
                new[] { new PermissionsRequirement(GrantedPermissions, PermissionRequireBehavior.Any) },
                true
            };

            yield return new object[]
            {
                new[] { new PermissionsRequirement(mixedPermissions, PermissionRequireBehavior.Any) },
                true
            };

            yield return new object[]
            {
                new[] { new PermissionsRequirement(UngrantedPermissions, PermissionRequireBehavior.All) },
                false
            };

            yield return new object[]
            {
                new[] { new PermissionsRequirement(GrantedPermissions, PermissionRequireBehavior.All) },
                true
            };

            yield return new object[]
            {
                new[] { new PermissionsRequirement(mixedPermissions, PermissionRequireBehavior.All) },
                false
            };
        }

        [Fact]
        public async Task HandleRequirementAsync_WhenNotAuthenticated_Failed()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var context = new AuthorizationHandlerContext(
                new[] { new PermissionsRequirement(GrantedPermissions, PermissionRequireBehavior.Any) },
                principal,
                null);
            await AuthorizationHandler.HandleAsync(context);

            principal.Identity.IsAuthenticated.Should().BeFalse();
            context.HasFailed.Should().BeTrue();
        }

        [MemberData(nameof(GetRequirements))]
        [Theory]
        public async Task HandleRequirementAsync_WhenAuthenticated_HandleCorrectly(
            PermissionsRequirement[] requirements,
            bool hasSucceed)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }, "basic");
            var principal = new ClaimsPrincipal(identity);
            var context = new AuthorizationHandlerContext(
                requirements,
                principal,
                null);
            await AuthorizationHandler.HandleAsync(context);

            principal.Identity.IsAuthenticated.Should().BeTrue();
            context.HasSucceeded.Should().Be(hasSucceed);
        }
    }
}