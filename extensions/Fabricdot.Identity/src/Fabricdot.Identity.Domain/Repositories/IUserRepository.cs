using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Domain.Services;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Repositories
{
    public interface IUserRepository<TUser> : IRepository<TUser, Guid> where TUser : IdentityUser
    {
        Task<TUser> GetDetailsByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<TUser> GetByNormalizedUserNameAsync(
            string normalizedUserName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task<TUser> GetByNormalizedEmailAsync(
            string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task<TUser> GetByLoginAsync(
            string loginProvider,
            string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<string>> ListRoleNamesAsync<TRole>(
            Guid id,
            CancellationToken cancellationToken = default) where TRole : IdentityRole;

        Task<IReadOnlyCollection<TUser>> ListAsync(
            ICollection<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TUser>> ListByClaimAsync(
            string claimType,
            string claimValue,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<TUser>> ListByRoleIdAsync(
            Guid roleId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);
    }
}