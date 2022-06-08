using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Services;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Domain.Repositories;

public interface IRoleRepository<TRole> : IRepository<TRole, Guid> where TRole : IdentityRole
{
    Task<TRole> GetDetailsByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<TRole> GetByNormalizedNameAsync(
        string nortmalizedName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TRole>> ListAsync(
        ICollection<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);
}