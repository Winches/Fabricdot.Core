using System.Collections.Immutable;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization;

[Dependency(ServiceLifetime.Transient)]
public class NullPermissionGrantingService : IPermissionGrantingService
{
    public Task<IReadOnlySet<GrantResult>> IsGrantedAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default)
    {
        var ret = objects.Select(v => new GrantResult(v, true)).ToImmutableHashSet().As<IReadOnlySet<GrantResult>>()!;
        return Task.FromResult(ret);
    }
}