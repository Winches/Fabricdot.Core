using Fabricdot.Domain.Services;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Domain.Repositories;

public interface IRoleRepository<TRole> : IRepository<TRole, Guid> where TRole : IdentityRole
{
    [Obsolete("Use 'GetByIdAsync'")]
    Task<TRole?> GetDetailsByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TRole?> GetByNormalizedNameAsync(
        string nortmalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);
}