using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Authorization.Permissions;

namespace Fabricdot.Authorization
{
    public static class PermissionGrantingServiceExtensions
    {
        public static async Task<bool> IsGrantedAsync(
            this IPermissionGrantingService permissionGrantingService,
            GrantSubject subject,
            string @object,
            CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(permissionGrantingService, nameof(permissionGrantingService));

            var grantResults = await permissionGrantingService.IsGrantedAsync(
                subject,
                new[] { @object },
                cancellationToken);

            return grantResults.First().IsGranted;
        }

        public static async Task<bool> IsSuperuserAsync(
            this IPermissionGrantingService permissionGrantingService,
            GrantSubject subject,
            CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(permissionGrantingService, nameof(permissionGrantingService));

            return await permissionGrantingService.IsGrantedAsync(
                subject,
                StandardPermissions.Superuser,
                cancellationToken);
        }
    }
}