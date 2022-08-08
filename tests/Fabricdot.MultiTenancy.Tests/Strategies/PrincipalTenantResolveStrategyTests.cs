using System.Security.Claims;
using Fabricdot.Core.Security;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.MultiTenancy.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy.Tests.Strategies;

public class PrincipalTenantResolveStrategyTests : IntegrationTestBase<MultiTenancyTestModule>
{
    private readonly PrincipalTenantResolveStrategy _strategy;

    private readonly IPrincipalAccessor _principalAccessor;

    public PrincipalTenantResolveStrategyTests()
    {
        _strategy = new PrincipalTenantResolveStrategy();
        _principalAccessor = ServiceProvider.GetRequiredService<IPrincipalAccessor>();
    }

    [Fact]
    public async Task ResolveIdentifierAsync_WhenCurrentPrincipalIsNull_ReturnNull()
    {
        using var scope = _principalAccessor.Change(null);
        var context = Create<TenantResolveContext>();
        var identifier = await _strategy.ResolveIdentifierAsync(context);

        identifier.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineAutoData]
    public async Task ResolveIdentifierAsync_WhenCurrentPrincipalIsNotNull_ReturnCorrectly(string tenantId)
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
            new[] { new Claim(TenantClaimTypes.TenantId, tenantId) },
            Create<string>()));

        using var scope = _principalAccessor.Change(claimsPrincipal);
        var context = Create<TenantResolveContext>();
        var identifier = await _strategy.ResolveIdentifierAsync(context);

        identifier.Should().Be(tenantId);
    }
}