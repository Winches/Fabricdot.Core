using System.Threading.Tasks;
using Fabricdot.Authorization.Permissions;
using Fabricdot.Core.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization
{
    [Dependency(ServiceLifetime.Scoped)]
    [ServiceContract(typeof(IAuthorizationHandler))]
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected IPermissionEvaluator PermissionEvaluator { get; }

        public PermissionAuthorizationHandler(IPermissionEvaluator permissionService)
        {
            PermissionEvaluator = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var principal = context.User;
            if ((principal?.Identity?.IsAuthenticated ?? false)
                && await PermissionEvaluator.EvaluateAsync(principal, requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }
    }
}