using Fabricdot.Domain.Internal;
using Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

namespace Fabricdot.MultiTenancy.Tests;

public class MultiTenantInitializerTests : TestBase
{
    private readonly ITenantAccessor _tenantAccessor;

    public MultiTenantInitializerTests()
    {
        _tenantAccessor = DefaultTenantAccessor.Instance;
        EntityInitializer.Instance.Add<MultiTenantInitializer>();
    }

    [AutoData]
    [Theory]
    internal void Construct_Entity_SetTenantId(TenantInfo tenant)
    {
        using var scope1 = _tenantAccessor.Change(tenant);
        var entity1 = Create<Customer>();

        entity1.TenantId.Should().Be(tenant.Id);

        using var scope2 = _tenantAccessor.Change(null);
        var entity2 = Create<Customer>();

        entity2.TenantId.Should().BeNull();
    }

    [AutoData]
    [Theory]
    internal void Construct_GivenTenantId_OverwriteTenantId(
        [Greedy] Customer entity,
        TenantInfo tenant)
    {
        var tenantId = entity.TenantId!.Value;
        using var scope = _tenantAccessor.Change(tenant);

        tenant.Id.Should().NotBe(tenantId);
        entity.TenantId.Should().Be(tenantId);
    }
}
