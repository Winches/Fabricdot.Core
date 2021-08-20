using System.Security.Claims;
using Fabricdot.Core.Security;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Core.Services
{
    public class HttpContextCurrentPrincipalAccessor : DefaultCurrentPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal() => _httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
    }
}