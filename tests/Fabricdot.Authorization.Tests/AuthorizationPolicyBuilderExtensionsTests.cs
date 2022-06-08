using System.Linq;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace Fabricdot.Authorization.Tests;

public class AuthorizationPolicyBuilderExtensionsTests
{
    [Fact]
    public void RequirePermission_GivenName_AddRequirement()
    {
        var permission = new PermissionName("name1");
        var builder = new AuthorizationPolicyBuilder().RequirePermission(permission);

        builder.Requirements.OfType<PermissionRequirement>()
                            .Should()
                            .ContainSingle(v => v.Permission == permission);
    }

    [InlineData(PermissionRequireBehavior.Any)]
    [InlineData(PermissionRequireBehavior.All)]
    [Theory]
    public void RequirePermissions_GivenNames_AddRequirement(PermissionRequireBehavior requireBehavior)
    {
        var permissions = new[] { new PermissionName("name1"), new PermissionName("name2") };
        var builder = new AuthorizationPolicyBuilder().RequirePermissions(permissions, requireBehavior);

        builder.Requirements.OfType<PermissionsRequirement>()
                            .Should()
                            .SatisfyRespectively(first => first.Should().BeEquivalentTo(new
                            {
                                Permissions = permissions
                            }));
    }
}