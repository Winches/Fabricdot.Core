using Fabricdot.Core.DependencyInjection;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.MultiTenancy;

[Dependency(ServiceLifetime.Transient, RegisterBehavior = RegistrationBehavior.Replace)]
public class CurrentTenant : ICurrentTenant
{
    private readonly ITenantAccessor _tenantAccessor;

    public Guid? Id => _tenantAccessor.Tenant?.Id;

    public string? Name => _tenantAccessor.Tenant?.Name;

    public bool IsAvailable => Id.HasValue;

    public CurrentTenant(ITenantAccessor tenantAccessor)
    {
        _tenantAccessor = tenantAccessor;
    }

    public IDisposable Change(
        Guid? tenantId,
        string? tenantName = null)
    {
        var tenant = tenantId.HasValue
            ? new TenantInfo(tenantId.Value, tenantName ?? string.Empty)
            : null;
        return _tenantAccessor.Change(tenant);
    }
}