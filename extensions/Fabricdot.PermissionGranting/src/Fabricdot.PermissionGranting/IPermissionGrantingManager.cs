using System.Security.Claims;
using Fabricdot.Authorization;
using Fabricdot.PermissionGranting.Domain;

namespace Fabricdot.PermissionGranting;

public interface IPermissionGrantingManager
{
    /// <summary>
    ///     Grant <paramref name="object" /> to <paramref name="subject" />
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="object"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task GrantAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Revoke <paramref name="object" /> to <paramref name="subject" />
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="object"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAsync(
        GrantSubject subject,
        string @object,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Set <paramref name="subject" /> with <paramref name="objects" />
    /// </summary>
    /// <remarks>
    ///     It wiil replace all granted objects with given <paramref name="objects" />
    /// </remarks>
    /// <param name="subject"></param>
    /// <param name="objects"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetAsync(
        GrantSubject subject,
        IEnumerable<string> objects,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     List all granted objects of <paramref name="subject" />
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        GrantSubject subject,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     List all granted objects of <paramref name="subjects" />
    /// </summary>
    /// <param name="subjects"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        ICollection<GrantSubject> subjects,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     List all granted objects of <paramref name="claimsPrincipal" />
    /// </summary>
    /// <remarks>
    ///     Use <see cref="IGrantSubjectResolver" /> to resolve subjects of <paramref
    ///     name="claimsPrincipal" />
    /// </remarks>
    /// <param name="claimsPrincipal"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<GrantedPermission>> ListAsync(
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken = default);
}