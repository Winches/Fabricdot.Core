using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy.Tests;

public class CurrentTenantTests : TestFor<CurrentTenant>
{
    protected static ITenant CurrentTenant { get; set; }

    public CurrentTenantTests()
    {
        var tenantAccessorMock = InjectMock<ITenantAccessor>();
        tenantAccessorMock.SetupGet(v => v.Tenant).Returns(() => CurrentTenant);
    }

    [InlineAutoData(null)]
    [InlineAutoData]
    [Theory]
    internal void Id_GivenTenant_ReturnCorrectly(TenantInfo tenant)
    {
        CurrentTenant = tenant;

        Sut.Id.Should().Be(CurrentTenant?.Id);
    }

    [InlineAutoData(null)]
    [InlineAutoData]
    [Theory]
    internal void Name_GivenTenant_ReturnCorrectly(TenantInfo tenant)
    {
        CurrentTenant = tenant;

        Sut.Name.Should().Be(CurrentTenant?.Name);
    }

    [InlineAutoData(null)]
    [InlineAutoData]
    [Theory]
    internal void IsAvailable_GivenTenant_ReturnCorrectly(TenantInfo tenant)
    {
        CurrentTenant = tenant;
        var expected = CurrentTenant?.Id != null;

        Sut.IsAvailable.Should().Be(expected);
    }
}