using System.Security.Claims;

namespace Fabricdot.Authorization.Permissions;

public interface IPermissionEvaluator
{
    Task<IReadOnlySet<GrantResult>> EvaluateAsync(
        ClaimsPrincipal principal,
        IEnumerable<PermissionName> permissions,
        CancellationToken cancellationToken = default);
}
