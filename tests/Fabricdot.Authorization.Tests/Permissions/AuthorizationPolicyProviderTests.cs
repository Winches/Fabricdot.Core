using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.Authorization.Tests.Permissions;

public class AuthorizationPolicyProviderTests : IntegrationTestBase<AuthorizationTestModule>
{
    protected AuthorizationOptions AuthorizationOptions { get; }

    protected IAuthorizationPolicyProvider AuthorizationPolicyProvider { get; }

    public AuthorizationPolicyProviderTests()
    {
        AuthorizationOptions = ServiceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
        AuthorizationPolicyProvider = ServiceProvider.GetRequiredService<IAuthorizationPolicyProvider>();
    }

    [AutoData]
    [Theory]
    public async Task GetPolicyAsync_GivenNonExistsPolicy_BuildPolicy(string policyName)
    {
        var policy = await AuthorizationPolicyProvider.GetPolicyAsync(policyName);

        AuthorizationOptions.GetPolicy(policyName).Should().Be(policy);
        policy.Should().NotBeNull();
        policy!.Requirements.Should()
                            .ContainSingle().Which
                            .Should()
                            .BeOfType<PermissionRequirement>();
    }

    [AutoData]
    [Theory]
    public async Task GetPolicyAsync_GivenExistsPolicy_ReturnPolicy(string policyName)
    {
        var expected = new AuthorizationPolicyBuilder().RequirePermission(policyName).Build();
        AuthorizationOptions.AddPolicy(policyName, expected);
        var authorizationPolicy = await AuthorizationPolicyProvider.GetPolicyAsync(policyName);

        authorizationPolicy.Should().BeSameAs(expected);
    }
}
