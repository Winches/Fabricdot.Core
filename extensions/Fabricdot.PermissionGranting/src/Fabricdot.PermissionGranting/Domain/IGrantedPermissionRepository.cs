using Fabricdot.Authorization;
using Fabricdot.Domain.Services;

namespace Fabricdot.PermissionGranting.Domain;

public interface IGrantedPermissionRepository : IRepository<GrantedPermission, Guid>
{
    Task<bool> AnyAsync(
            GrantSubject subject,
            string @object,
            CancellationToken cancellationToken = default);

    Task<GrantedPermission?> GetAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        IEnumerable<GrantSubject> subjects,
        CancellationToken cancellationToken = default);
}
