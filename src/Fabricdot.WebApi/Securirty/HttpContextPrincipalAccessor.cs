using System.Security.Claims;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Securirty
{
    [Dependency(ServiceLifetime.Singleton, RegisterBehavior = RegistrationBehavior.Replace)]
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