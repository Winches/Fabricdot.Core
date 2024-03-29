using Ardalis.GuardClauses;

namespace Fabricdot.MultiTenancy;

public class TenantResolveContext
{
    public IServiceProvider ServiceProvider { get; }

    public TenantResolveContext(IServiceProvider serviceProvider)
    {
        Guard.Against.Null(serviceProvider, nameof(serviceProvider));
        ServiceProvider = serviceProvider;
    }
}
