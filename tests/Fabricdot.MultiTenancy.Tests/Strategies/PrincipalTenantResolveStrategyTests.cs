using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Core.Security;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.MultiTenancy.Strategies;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Fabricdot.MultiTenancy.Tests.Strategies
{
    public class PrincipalTenantResolveStrategyTests : IntegrationTestBase
    {
        private readonly PrincipalTenantResolveStrategy _strategy;
        protected static ClaimsPrincipal CurrentPrincipal { get; set; }

        public PrincipalTenantResolveStrategyTests()
        {
            _strategy = new PrincipalTenantResolveStrategy();
        }

        [Fact]
        public async Task ResolveIdentifierAsync_WhenCurrentPrincipalIsNull_ReturnNull()
        {
            CurrentPrincipal = null;
            var context = new TenantResolveContext(ServiceScope.ServiceProvider);
            var identifier = await _strategy.ResolveIdentifierAsync(context);

            Assert.Null(identifier);
        }

        [Theory]
        [InlineData("")]
        [InlineData("7f32fca3-efa6-4e9c-a5a8-2c278c26f032")]
        public async Task ResolveIdentifierAsync_WhenCurrentPrincipalIsNotNull_ReturnCorrectly(string tenantId)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            var claimsIdentity = new ClaimsIdentity(
                new[] { new Claim(TenantClaimTypes.TenantId, tenantId) },
                "Bearer");
            claimsPrincipal.AddIdentity(claimsIdentity);
            CurrentPrincipal = claimsPrincipal;
            var context = new TenantResolveContext(ServiceScope.ServiceProvider);
            var identifier = await _strategy.ResolveIdentifierAsync(context);

            Assert.Equal(identifier, tenantId);
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var mock = new Mock<IPrincipalAccessor>();
            mock.SetupGet(v => v.Principal).Returns(() => CurrentPrincipal);
            serviceCollection.AddScoped(_ => mock.Object);
            new MultiTenancyModule().Configure(serviceCollection);
        }
    }
}