using System;
using Fabricdot.Domain.Internal;
using Fabricdot.MultiTenancy.Abstractions;
using Fabricdot.MultiTenancy.Tests.Entities;
using Xunit;

namespace Fabricdot.MultiTenancy.Tests;

public class MultiTenantInitializerTests
{
    private readonly ITenantAccessor _tenantAccessor;

    public MultiTenantInitializerTests()
    {
        _tenantAccessor = DefaultTenantAccessor.Instance;
        EntityInitializer.Instance.Add<MultiTenantInitializer>();
    }

    [Fact]
    public void Construct_Entity_SetTenantId()
    {
        var tenant1 = new TenantInfo(Guid.NewGuid(), "tenant1");
        using var scope1 = _tenantAccessor.Change(tenant1);
        var entity1 = new MultiTenantEmployee("name1");
        Assert.Equal(tenant1.Id, entity1.TenantId);
        using var scope2 = _tenantAccessor.Change(null);
        var entity2 = new MultiTenantEmployee("name2");
        Assert.Null(entity2.TenantId);
    }

    [Fact]
    public void Construct_GivenTenantId_OverwriteTenantId()
    {
        var tenantId = Guid.NewGuid();
        var tenant1 = new TenantInfo(Guid.NewGuid(), "tenant1");
        using var scope1 = _tenantAccessor.Change(tenant1);
        var entity = new MultiTenantEmployee("name1", tenantId);
        Assert.Equal(tenantId, entity.TenantId);
        Assert.NotEqual(tenant1.Id, entity.TenantId);
    }
}