using System.Diagnostics;
using System.Reflection;
using Fabricdot.Core.Reflection;
using Fabricdot.Domain.Internal;
using Fabricdot.MultiTenancy.Abstractions;

namespace Fabricdot.MultiTenancy
{
    public class MultiTenantInitializer : IEntityInitializer
    {
        public void Initialize(object entity)
        {
            var tenantId = DefaultTenantAccessor.Instance.Tenant?.Id;
            if (entity is not IMultiTenant multiTenant)
                return;
            if (multiTenant.TenantId.HasValue)
                return;
            if (multiTenant.TenantId == tenantId)
                return;

            var propertyInfo = multiTenant.GetType().GetProperty(
                nameof(IMultiTenant.TenantId),
                BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo?.CanWrite == true)
            {
                propertyInfo.SetValue(entity, tenantId);
            }
            Debug.Print($"Initialize '{entity.GetType().PrettyPrint()}' with tenant:{tenantId}");
        }
    }
}