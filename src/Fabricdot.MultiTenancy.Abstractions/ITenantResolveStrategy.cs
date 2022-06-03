using System.Threading.Tasks;

namespace Fabricdot.MultiTenancy.Abstractions
{
    public interface ITenantResolveStrategy
    {
        int Priority { get; }

        Task<string?> ResolveIdentifierAsync(TenantResolveContext context);
    }
}