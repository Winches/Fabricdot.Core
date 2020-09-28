using System.Linq;
using System.Security.Claims;
using Fabricdot.Common.Core.Security;

namespace Fabricdot.Infrastructure.Core.Security
{
    public class CurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public virtual bool IsAuthenticated => !string.IsNullOrEmpty(Id);

        public virtual string Id => _principalAccessor.Principal?.Claims
            ?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value ?? string.Empty;

        public virtual string UserName => FindClaim(ClaimTypes.Name)?.Value ?? string.Empty;

        public virtual string[] Roles => FindClaims(ClaimTypes.Role).Select(c => c.Value).ToArray();

        public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return _principalAccessor.Principal?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        public virtual Claim[] GetAllClaims()
        {
            return _principalAccessor.Principal?.Claims.ToArray() ?? EmptyClaimsArray;
        }

        public virtual bool IsInRole(string roleName)
        {
            return FindClaims(ClaimTypes.Role).Any(c => c.Value == roleName);
        }
    }
}