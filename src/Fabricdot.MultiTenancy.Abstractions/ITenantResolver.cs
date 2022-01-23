using System.Threading.Tasks;

namespace Fabricdot.MultiTenancy.Abstractions
{
    public interface ITenantResolver
    {
        Task<TenantResolveResult> ResolveAsync();
    }
}