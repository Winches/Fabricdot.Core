using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Authorization.Permissions;

public interface IPermissionHandlerContextFactory
{
    Task<PermissionHandlerContext> CreateAsync(
        ClaimsPrincipal claimsPrincipal,
        IEnumerable<PermissionName> permissions,
        CancellationToken cancellationToken = default);
}