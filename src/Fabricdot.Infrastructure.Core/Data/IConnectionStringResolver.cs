using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface IConnectionStringResolver
    {
        Task<string> ResolveAsync(string name = null);
    }
}