using System;
using System.Linq;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Specifications
{
    public class UserPagedListSpecification<TUser> : UserWithDetailsSpecification<TUser> where TUser : IdentityUser
    {
        public UserPagedListSpecification(
            int index,
            int size,
            string filter = null,
            bool? isActive = null,
            bool? isLockedOut = null,
            bool includeDetails = false) : base(includeDetails)
        {
            Guard.Against.NegativeOrZero(index, nameof(index));
            Guard.Against.NegativeOrZero(size, nameof(size));
            filter = filter?.Trim();

            if (!string.IsNullOrEmpty(filter))
            {
                Query.Where(v => v.UserName.Contains(filter)
                                 || v.NormalizedUserName.Contains(filter)
                                 || (v.GivenName != null && v.GivenName.Contains(filter))
                                 || (v.Surname != null && v.Surname.Contains(filter))
                                 || (v.Email != null && v.Email.Contains(filter))
                                 || (v.NormalizedEmail != null && v.NormalizedEmail.Contains(filter))
                                 || v.PhoneNumber.Contains(filter));
            }

            if (isLockedOut.HasValue)
            {
                Query.Where(v => isLockedOut.Value
                    ? v.LockoutEnabled && v.LockoutEnd >= DateTimeOffset.UtcNow
                    : !v.LockoutEnabled || v.LockoutEnd < DateTimeOffset.UtcNow);
            }

            if (isActive.HasValue)
                Query.Where(v => v.IsActive == isActive);

            Query.OrderByDescending(v => v.CreationTime);

            Query.Skip((index - 1) * size)
                 .Take(size);
        }
    }
}