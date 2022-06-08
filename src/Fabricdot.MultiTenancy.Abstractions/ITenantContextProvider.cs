using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.MultiTenancy.Abstractions;

public interface ITenantContextProvider
{
    Task<TenantContext?> GetAsync(CancellationToken cancellationToken = default);
}