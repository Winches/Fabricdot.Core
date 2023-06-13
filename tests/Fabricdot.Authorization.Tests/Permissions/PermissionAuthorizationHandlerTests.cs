using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionAuthorizationHandlerTests : AuthrizationHandlerTestsBase<PermissionAuthorizationHandler>
{
    [Fact]
    public async Task HandleRequirementAsync_WhenNotAuthenticated_Failed()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext(
            GrantedPermissions.Select(v => new PermissionRequirement(v)),
            principal,
            null);
        await AuthorizationHandler.HandleAsync(context);

        principal.Identity!.IsAuthenticated.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenAuthenticated_HandleCorrectly()
    {
        var identity = new ClaimsIdentity(new[] { Superuser }, Create<string>());

        var principal = new ClaimsPrincipal(identity);
        var context = new AuthorizationHandlerContext(
            GrantedPermissions.Select(v => new PermissionRequirement(v)).ToArray(),
            principal,
            null);
        await AuthorizationHandler.HandleAsync(context);

        principal.Identity!.IsAuthenticated.Should().BeTrue();
        context.HasSucceeded.Should().BeTrue();
    }
}
