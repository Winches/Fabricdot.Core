namespace Fabricdot.Authorization;

public interface IPermissionGrantingService
{
    Task<IReadOnlySet<GrantResult>> IsGrantedAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default);
}