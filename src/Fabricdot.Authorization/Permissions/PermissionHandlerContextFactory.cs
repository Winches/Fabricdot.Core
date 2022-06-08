using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization.Permissions;

[Dependency(ServiceLifetime.Transient)]
public class PermissionHandlerContextFactory : IPermissionHandlerContextFactory
{
    private readonly IGrantSubjectResolver _grantSubjectResolver;

    public PermissionHandlerContextFactory(IGrantSubjectResolver grantSubjectResolver)
    {
        _grantSubjectResolver = grantSubjectResolver;
    }

    public async Task<PermissionHandlerContext> CreateAsync(
        ClaimsPrincipal claimsPrincipal,
        IEnumerable<PermissionName> permissions,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(claimsPrincipal, nameof(claimsPrincipal));
        Guard.Against.NullOrEmpty(permissions, nameof(permissions));

        var subjects = await _grantSubjectResolver.ResolveAsync(claimsPrincipal, cancellationToken);

        return new PermissionHandlerContext(subjects, permissions);
    }
}