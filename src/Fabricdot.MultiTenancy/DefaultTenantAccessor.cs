using Fabricdot.Core.Delegates;

namespace Fabricdot.MultiTenancy;

public class DefaultTenantAccessor : ITenantAccessor
{
    public static readonly DefaultTenantAccessor Instance = new();

    private readonly AsyncLocal<ITenant?> _tenant = new();

    public ITenant? Tenant => _tenant.Value;

    private DefaultTenantAccessor()
    {
    }

    public virtual IDisposable Change(ITenant? tenant) => SetCurrent(tenant);

    protected IDisposable SetCurrent(ITenant? tenant)
    {
        var parent = Tenant;
        _tenant.Value = tenant;
        return new DisposeAction(() => _tenant.Value = parent);
    }
}
