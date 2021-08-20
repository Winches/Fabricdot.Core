using System;
using System.Security.Claims;
using System.Threading;
using Fabricdot.Core.Delegates;

namespace Fabricdot.Core.Security
{
    public class DefaultCurrentPrincipalAccessor : ICurrentPrincipalAccessor
    {
        private readonly AsyncLocal<ClaimsPrincipal> _currentPrincipal = new AsyncLocal<ClaimsPrincipal>();

        public ClaimsPrincipal Principal => _currentPrincipal.Value ?? GetClaimsPrincipal();

        public virtual IDisposable Change(ClaimsPrincipal principal) => SetCurrent(principal);

        protected virtual ClaimsPrincipal GetClaimsPrincipal() => Thread.CurrentPrincipal as ClaimsPrincipal;

        protected IDisposable SetCurrent(ClaimsPrincipal principal)
        {
            var parent = Principal;
            _currentPrincipal.Value = principal;
            return new DisposeAction(() => _currentPrincipal.Value = parent);
        }
    }
}