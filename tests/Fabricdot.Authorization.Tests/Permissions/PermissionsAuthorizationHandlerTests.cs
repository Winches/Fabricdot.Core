using System.Security.Claims;
using Fabricdot.Core.Security;
using Microsoft.AspNetCore.Authorization;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionsAuthorizationHandlerTests : AuthrizationHandlerTestsBase<PermissionsAuthorizationHandler>
{
    [InlineData(PermissionRequireBehavior.All)]
    [InlineData(PermissionRequireBehavior.Any)]
    [Theory]
    public async Task HandleRequirement_WhenNotAuthenticated_Failed(PermissionRequireBehavior requireBehavior)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext(
            new[] { new PermissionsRequirement(GrantedPermissions, requireBehavior) },
            principal,
            null);
        await AuthorizationHandler.HandleAsync(context);

        principal.Identity!.IsAuthenticated.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirement_Should_Failed()
    {
        var requirements = new[]
        {
            new PermissionsRequirement(UngrantedPermissions, PermissionRequireBehavior.Any),
            new PermissionsRequirement(UngrantedPermissions, PermissionRequireBehavior.All),
            new PermissionsRequirement(Permissions, PermissionRequireBehavior.All)
        };

        var identity = new ClaimsIdentity(
            Create<Claim[]>(),
            Create<string>());
        var principal = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(
            requirements,
            principal,
            null);
        await AuthorizationHandler.HandleAsync(context);

        principal.Identity!.IsAuthenticated.Should().BeTrue();
        context.HasSucceeded.Should().BeFalse();
        context.PendingRequirements.Should().HaveSameCount(requirements);
    }

    [Fact]
    public async Task HandleRequirement_Should_Success()
    {
        var requirements = new[]
        {
            new PermissionsRequirement(Permissions, PermissionRequireBehavior.Any),
            new PermissionsRequirement(GrantedPermissions, PermissionRequireBehavior.All),
        };

        var identity = new ClaimsIdentity(
            //Create<Claim[]>(),
            new[] { new Claim(SharedClaimTypes.NameIdentifier, Create<string>()) },
            Create<string>());
        var principal = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(
            requirements,
            principal,
            null);
        await AuthorizationHandler.HandleAsync(context);

        principal.Identity!.IsAuthenticated.Should().BeTrue();
        context.HasSucceeded.Should().BeTrue();
    }
}
