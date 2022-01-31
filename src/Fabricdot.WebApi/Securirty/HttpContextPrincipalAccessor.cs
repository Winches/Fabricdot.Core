using System.Security.Claims;
using Fabricdot.Core.Security;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Securirty
{
    public class HttpContextPrincipalAccessor : DefaultPrincipalAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override ClaimsPrincipal GetClaimsPrincipal() => _httpContextAccessor.HttpContext?.User ?? base.GetClaimsPrincipal();
    }
}