using System.Security.Claims;

namespace Fabricdot.Authorization;

public interface IGrantSubjectResolver
{
    Task<ICollection<GrantSubject>> ResolveAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default);
}