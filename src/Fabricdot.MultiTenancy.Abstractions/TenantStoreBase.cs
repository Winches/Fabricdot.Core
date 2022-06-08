using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.MultiTenancy.Abstractions;

public abstract class TenantStoreBase : ITenantStore
{
    public abstract Task<TenantContext> GetAsync(
        string identifier,
        CancellationToken cancellationToken = default);
}