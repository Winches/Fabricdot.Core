using System;
using System.Security.Claims;
using System.Threading;
using Fabricdot.Core.Delegates;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Security;

[Dependency(ServiceLifetime.Singleton)]
public class DefaultPrincipalAccessor : IPrincipalAccessor
{
    private readonly AsyncLocal<ClaimsPrincipal?> _currentPrincipal = new();

    public ClaimsPrincipal? Principal => _currentPrincipal.Value ?? GetClaimsPrincipal();

    public virtual IDisposable Change(ClaimsPrincipal? principal) => SetCurrent(principal);

    protected virtual ClaimsPrincipal? GetClaimsPrincipal() => Thread.CurrentPrincipal as ClaimsPrincipal;

    protected IDisposable SetCurrent(ClaimsPrincipal? principal)
    {
        var parent = Principal;
        _currentPrincipal.Value = principal;
        return new DisposeAction(() => _currentPrincipal.Value = parent);
    }
}