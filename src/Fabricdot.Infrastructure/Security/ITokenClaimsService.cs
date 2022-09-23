namespace Fabricdot.Infrastructure.Security;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}