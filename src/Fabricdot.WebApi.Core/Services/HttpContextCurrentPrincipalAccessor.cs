using System;
using System.Security.Claims;
using System.Threading;
using Fabricdot.Core.Delegates;
using Fabricdot.Core.Security;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace Fabricdot.WebApi.Core.Services
{
    public class HttpContextCurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        private readonly AsyncLocal<ClaimsPrincipal> _currentPrincipal = new AsyncLocal<ClaimsPrincipal>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimsPrincipal Principal => _currentPrincipal.Value ?? GetClaimsPrincipal();

        public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual IDisposable Change([NotNull] ClaimsPrincipal principal)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));
            return SetCurrent(principal);
        }

        protected ClaimsPrincipal GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User ?? Thread.CurrentPrincipal as ClaimsPrincipal;
        }

        private IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            _currentPrincipal.Value = principal;
            return new DisposeAction(() => { _currentPrincipal.Value = parent; });
        }
    }
}