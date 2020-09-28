using System;
using System.Security.Claims;

namespace Fabricdot.Common.Core.Security
{
    public interface ICurrentPrincipalAccessor
    {
        ClaimsPrincipal Principal { get; }

        IDisposable Change(ClaimsPrincipal principal);
    }
}