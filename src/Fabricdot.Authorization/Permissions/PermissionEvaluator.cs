using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Authorization.Permissions;

[Dependency(ServiceLifetime.Scoped)]
public class PermissionEvaluator : IPermissionEvaluator
{
    public ILogger<PermissionEvaluator> Logger { get; }
    protected IPermissionGrantingService PermissionGrantingService { get; }
    protected IPermissionHandlerContextFactory AuthorizationContextFactory { get; }
    protected IPermissionManager PermissionManager { get; }

    public PermissionEvaluator(
        ILogger<PermissionEvaluator> logger,
        IPermissionGrantingService permissionGrantingService,
        IPermissionHandlerContextFactory authorizationContextFactory,
        IPermissionManager permissionManager)
    {
        Logger = logger;
        PermissionGrantingService = permissionGrantingService;
        AuthorizationContextFactory = authorizationContextFactory;
        PermissionManager = permissionManager;
    }

    public virtual async Task<IReadOnlySet<GrantResult>> EvaluateAsync(
        ClaimsPrincipal principal,
        IEnumerable<PermissionName> permissions,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(principal, nameof(principal));
        Guard.Against.NullOrEmpty(permissions, nameof(permissions));

        await EnsurePermissionIsValidAsync(permissions);

        var authorizationContext = await AuthorizationContextFactory.CreateAsync(
            principal,
            permissions,
            cancellationToken);

        Logger.LogDebug($"Evaluate permissions: {authorizationContext.ToJson()}");

        foreach (var subject in authorizationContext.Subjects)
        {
            if (authorizationContext.PendingPermissions.Count == 0)
                break;

            var pendingPermissions = authorizationContext.PendingPermissions.ToList();
            pendingPermissions.AddIfNotContains(StandardPermissions.Superuser);

            var grantResults = await PermissionGrantingService.IsGrantedAsync(
                subject,
                pendingPermissions.Select(v => v.ToString()),
                cancellationToken);

            if (grantResults.Any(v => v.Object == StandardPermissions.Superuser && v.IsGranted))
            {
                authorizationContext.Succeed();
                Logger.LogDebug($"{subject} is super user.");
                break;
            }

            grantResults.Where(v => v.IsGranted)
                        .ForEach(v => authorizationContext.Succeed(v.Object));
        }
        return authorizationContext.GetResults();
    }

    protected virtual async Task EnsurePermissionIsValidAsync(IEnumerable<PermissionName> permissions)
    {
        await permissions.ForEachAsync(async (v, _) =>
        {
            var permission = await PermissionManager.GetByNameAsync(v);
            if (permission is null)
                throw new PermissionNotDefinedException(v);
        });
    }
}