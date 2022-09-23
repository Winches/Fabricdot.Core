using System.Security.Claims;
using Ardalis.GuardClauses;

namespace Fabricdot.Authorization.Permissions;

public static class PermissionEvaluatorExtensions
{
    public static async Task<bool> EvaluateAsync(
        this IPermissionEvaluator permissionEvaluator,
        ClaimsPrincipal principal,
        PermissionName permission,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(permissionEvaluator, nameof(permissionEvaluator));

        var grantResults = await permissionEvaluator.EvaluateAsync(
            principal,
            new[] { permission },
            cancellationToken);

        return grantResults.Single().IsGranted;
    }
}