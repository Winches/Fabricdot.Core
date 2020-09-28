using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Core.Security
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName);
    }
}