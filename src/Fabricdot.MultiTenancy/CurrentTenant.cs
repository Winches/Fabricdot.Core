using System;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly ITenantAccessor _tenantAccessor;

        public Guid? Id => _tenantAccessor.Tenant?.Id;

        public string Name => _tenantAccessor.Tenant?.Name;

        public bool IsAvailable => Id.HasValue;

        public CurrentTenant(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public IDisposable Change(
            Guid? tenantId,
            string tenantName = null)
        {
            var tenant = tenantId.HasValue
                ? new TenantInfo(tenantId.Value, tenantName)
                : null;
            return _tenantAccessor.Change(tenant);
        }
    }
}