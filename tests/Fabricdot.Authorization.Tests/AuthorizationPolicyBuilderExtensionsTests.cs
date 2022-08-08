using Fabricdot.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Fabricdot.Authorization.Tests;

public class AuthorizationPolicyBuilderExtensionsTests : TestFor<AuthorizationPolicyBuilder>
{
    [AutoData]
    [Theory]
    public void RequirePermission_GivenName_AddRequirement(PermissionName permission)
    {
        Sut.RequirePermission(permission);

        Sut.Requirements.OfType<PermissionRequirement>()
                            .Should()
                            .ContainSingle(permission);
    }

    [InlineAutoData(PermissionRequireBehavior.Any)]
    [InlineAutoData(PermissionRequireBehavior.All)]
    [Theory]
    public void RequirePermissions_GivenNames_AddRequirement(
        PermissionRequireBehavior requireBehavior,
        PermissionName[] permissions)
    {
        var expected = new
        {
            Permissions = permissions
        };
        Sut.RequirePermissions(permissions, requireBehavior);

        Sut.Requirements.OfType<PermissionsRequirement>()
                            .Should()
                            .SatisfyRespectively(first => first.Should().BeEquivalentTo(expected));
    }
}