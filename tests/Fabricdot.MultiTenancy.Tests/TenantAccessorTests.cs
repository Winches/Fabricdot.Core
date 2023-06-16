namespace Fabricdot.MultiTenancy.Tests;

public class TenantAccessorTests : TestFor<DefaultTenantAccessor>
{
    [AutoData]
    [Theory]
    internal void Change_GivenTenant_ChangeTenant(
        TenantInfo tenant1,
        TenantInfo tenant2)
    {
        using (var scope = Sut.Change(tenant1))
        {
            using (var scope2 = Sut.Change(tenant2))
            {
                Sut.Tenant.Should().Be(tenant2);
            }
            Sut.Tenant.Should().Be(tenant1);
        }
        Sut.Tenant.Should().BeNull();
    }

    protected override DefaultTenantAccessor CreateSut() => DefaultTenantAccessor.Instance;
}
