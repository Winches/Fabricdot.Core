using System.Security.Claims;

namespace Fabricdot.Infrastructure.Security
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        string Id { get; }

        string UserName { get; }

        string[] Roles { get; }

        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}