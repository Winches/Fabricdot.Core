using System;
using System.Collections.Generic;
using Fabricdot.MultiTenancy.Abstractions;
using Moq;
using Xunit;

namespace Fabricdot.MultiTenancy.Tests;

public class CurrentTenantTests
{
    private readonly ICurrentTenant _currentTenant;
    protected static ITenant CurrentTenant { get; set; }

    public CurrentTenantTests()
    {
        var tenantAccessor = new Mock<ITenantAccessor>();
        tenantAccessor.SetupGet(v => v.Tenant).Returns(() => CurrentTenant);
        _currentTenant = new CurrentTenant(tenantAccessor.Object);
    }

    public static IEnumerable<object[]> GetTenants()
    {
        yield return new object[]
        {
            null
        };
        yield return new object[]
        {
            new TenantInfo(Guid.NewGuid(), "tenant1")
        };
    }

    [Theory]
    [MemberData(nameof(GetTenants))]
    public void Id_GivenTenant_ReturnCorrectly(ITenant tenant)
    {
        CurrentTenant = tenant;
        Assert.Equal(_currentTenant.Id, CurrentTenant?.Id);
    }

    [Theory]
    [MemberData(nameof(GetTenants))]
    public void Name_GivenTenant_ReturnCorrectly(ITenant tenant)
    {
        CurrentTenant = tenant;
        Assert.Equal(_currentTenant.Name, CurrentTenant?.Name);
    }

    [Theory]
    [MemberData(nameof(GetTenants))]
    public void IsAvailable_GivenTenant_ReturnCorrectly(ITenant tenant)
    {
        CurrentTenant = tenant;
        Assert.Equal(_currentTenant.IsAvailable, CurrentTenant?.Id != null);
    }
}