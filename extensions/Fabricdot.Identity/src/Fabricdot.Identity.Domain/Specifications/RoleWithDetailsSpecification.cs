using Ardalis.Specification;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Domain.Specifications;

public class RoleWithDetailsSpecification<TRole> : Specification<TRole> where TRole : IdentityRole
{
    public RoleWithDetailsSpecification(bool includeDetails)
    {
        if (includeDetails)
        {
            Query.Include(v => v.Claims);
        }
    }
}
