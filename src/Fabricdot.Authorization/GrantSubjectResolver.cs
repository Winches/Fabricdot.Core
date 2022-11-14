using System.Security.Claims;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization;

[Dependency(ServiceLifetime.Transient)]
public class GrantSubjectResolver : IGrantSubjectResolver
{
    public virtual Task<ICollection<GrantSubject>> ResolveAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default)
    {
        Guard.Against.Null(claimsPrincipal, nameof(claimsPrincipal));

        var subjects = new List<GrantSubject>();

        var id = claimsPrincipal.FindFirst(SharedClaimTypes.NameIdentifier);
        if (id != null)
            subjects.Add(new GrantSubject(GrantTypes.User, id.Value));

        var roles = claimsPrincipal.FindAll(SharedClaimTypes.Role)
                                   .Select(v => v.Value)
                                   .Distinct()
                                   .Select(v => new GrantSubject(GrantTypes.Role, v));
        subjects.AddRange(roles);

        return Task.FromResult(subjects as ICollection<GrantSubject>);
    }
}