using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Authorization.Permissions;
using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization
{
    [Dependency(ServiceLifetime.Scoped)]
    [ServiceContract(typeof(IAuthorizationHandler))]
    public class PermissionsAuthorizationHandler : AuthorizationHandler<PermissionsRequirement>
    {
        protected IPermissionEvaluator PermissionService { get; }

        public PermissionsAuthorizationHandler(IPermissionEvaluator permissionService)
        {
            PermissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionsRequirement requirement)
        {
            var principal = context.User;

            if (principal.Identity.IsAuthenticated)
            {
                var grantResults = await PermissionService.EvaluateAsync(principal, requirement.Permissions);
                switch (requirement.RequireBehavior)
                {
                    case PermissionRequireBehavior.Any:
                        if (grantResults.Any(v => v.IsGranted))
                        {
                            context.Succeed(requirement);
                            return;
                        }

                        break;

                    case PermissionRequireBehavior.All:
                        if (grantResults.All(v => v.IsGranted))
                        {
                            context.Succeed(requirement);
                            return;
                        }

                        break;
                }
            }

            context.Fail();
        }
    }
}