using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Authorization.Permissions
{
    public interface IPermissionEvaluator
    {
        Task<IReadOnlySet<GrantResult>> EvaluateAsync(
            ClaimsPrincipal principal,
            IEnumerable<PermissionName> permissions,
            CancellationToken cancellationToken = default);
    }
}