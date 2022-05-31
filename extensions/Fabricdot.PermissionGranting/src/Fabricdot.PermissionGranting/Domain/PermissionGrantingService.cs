using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Authorization;
using Fabricdot.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Domain;

[Dependency(ServiceLifetime.Scoped, RegisterBehavior = RegistrationBehavior.Replace)]
public class PermissionGrantingService : IPermissionGrantingService
{
    protected IGrantedPermissionRepository PermissionGrantigRepository { get; }

    public PermissionGrantingService(IGrantedPermissionRepository permissionGrantigRepository)
    {
        PermissionGrantigRepository = permissionGrantigRepository;
    }

    public async Task<IReadOnlySet<GrantResult>> IsGrantedAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default)
    {
        var grants = await PermissionGrantigRepository.ListAsync(
            subject,
            objects,
            cancellationToken);

        var res = objects.Select(v => new GrantResult(v, grants.Any(o => o.Object == v)));
        return new HashSet<GrantResult>(res);
    }
}