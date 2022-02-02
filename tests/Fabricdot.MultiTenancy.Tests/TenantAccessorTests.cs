using System;
using Fabricdot.MultiTenancy.Abstractions;
using Xunit;

namespace Fabricdot.MultiTenancy.Tests
{
    public class TenantAccessorTests
    {
        private readonly ITenantAccessor _tenantAccessor;

        public TenantAccessorTests()
        {
            _tenantAccessor = DefaultTenantAccessor.Instance;
        }

        [Fact]
        public void Change_GivenTenant_ChangeTenant()
        {
            var tenant1 = new TenantInfo(Guid.NewGuid(), "tenant1");
            var tenant2 = new TenantInfo(Guid.NewGuid(), "tenant2");
            using (var scope = _tenantAccessor.Change(tenant1))
            {
                using (var scope2 = _tenantAccessor.Change(tenant2))
                {
                    Assert.Equal(_tenantAccessor.Tenant, tenant2);
                }
                Assert.Equal(_tenantAccessor.Tenant, tenant1);
            }
            Assert.Null(_tenantAccessor.Tenant);
        }
    }
}