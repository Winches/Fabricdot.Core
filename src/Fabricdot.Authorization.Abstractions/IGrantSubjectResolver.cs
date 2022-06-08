using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Fabricdot.Authorization;

public interface IGrantSubjectResolver
{
    Task<ICollection<GrantSubject>> ResolveAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default);
}