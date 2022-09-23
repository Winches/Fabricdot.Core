using System.Security.Claims;

namespace Fabricdot.Core.Security;

public interface IPrincipalAccessor
{
    ClaimsPrincipal? Principal { get; }

    IDisposable Change(ClaimsPrincipal? principal);
}