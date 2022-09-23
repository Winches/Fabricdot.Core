using System.Security.Claims;

namespace Fabricdot.Authorization.Permissions;

public interface IPermissionHandlerContextFactory
{
    Task<PermissionHandlerContext> CreateAsync(
        ClaimsPrincipal claimsPrincipal,
        IEnumerable<PermissionName> permissions,
        CancellationToken cancellationToken = default);
}