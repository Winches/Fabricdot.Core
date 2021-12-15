using System.Threading.Tasks;

namespace Fabricdot.Infrastructure.Security
{
    public interface ITokenClaimsService
    {
        Task<string> GetTokenAsync(string userName);
    }
}