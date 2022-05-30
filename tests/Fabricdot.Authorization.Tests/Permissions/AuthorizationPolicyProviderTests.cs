using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Test.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions
{
    public class AuthorizationPolicyProviderTests : IntegrationTestBase<AuthorizationTestModule>
    {
        protected AuthorizationOptions AuthorizationOptions { get; }

        protected IAuthorizationPolicyProvider AuthorizationPolicyProvider { get; }

        public AuthorizationPolicyProviderTests()
        {
            AuthorizationOptions = ServiceProvider.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
            AuthorizationPolicyProvider = ServiceProvider.GetRequiredService<IAuthorizationPolicyProvider>();
        }

        [Fact]
        public async Task GetPolicyAsync_GivenNonExistsPolicy_BuildPolicy()
        {
            const string policyName = "name1";
            var policy = await AuthorizationPolicyProvider.GetPolicyAsync(policyName);

            AuthorizationOptions.GetPolicy(policyName).Should().Be(policy);
            policy.Should().NotBeNull();
            policy.Requirements.Single().Should().BeOfType<PermissionRequirement>();
        }

        [Fact]
        public async Task GetPolicyAsync_GivenExistsPolicy_ReturnPolicy()
        {
            const string policyName = "name1";
            var policy = new AuthorizationPolicyBuilder().RequirePermission(policyName).Build();
            AuthorizationOptions.AddPolicy(policyName, policy);
            var authorizationPolicy = await AuthorizationPolicyProvider.GetPolicyAsync(policyName);

            authorizationPolicy.Should().BeSameAs(policy);
        }
    }
}