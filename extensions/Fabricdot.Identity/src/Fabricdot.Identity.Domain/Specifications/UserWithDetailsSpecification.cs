using Ardalis.Specification;
using Fabricdot.Identity.Domain.Entities.UserAggregate;

namespace Fabricdot.Identity.Domain.Specifications;

public class UserWithDetailsSpecification<TUser> : Specification<TUser> where TUser : IdentityUser
{
    public UserWithDetailsSpecification(bool includeDetails)
    {
        if (includeDetails)
        {
            Query.Include(v => v.Claims)
                 .Include(v => v.Logins)
                 .Include(v => v.Tokens)
                 .Include(v => v.Roles);
        }
    }
}