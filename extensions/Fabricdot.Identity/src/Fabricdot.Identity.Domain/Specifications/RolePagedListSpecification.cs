using System.Linq;
using Ardalis.GuardClauses;
using Ardalis.Specification;
using Fabricdot.Identity.Domain.Entities.RoleAggregate;

namespace Fabricdot.Identity.Domain.Specifications;

public class RolePagedListSpecification<TRole> : RoleWithDetailsSpecification<TRole> where TRole : IdentityRole
{
    public RolePagedListSpecification(
        int index,
        int size,
        string? name = null,
        bool includeDetails = false) : base(includeDetails)
    {
        Guard.Against.NegativeOrZero(index, nameof(index));
        Guard.Against.NegativeOrZero(size, nameof(size));
        name = name?.Trim();

        if (!string.IsNullOrEmpty(name))
            Query.Where(v => v.NormalizedName.Contains(name) || v.Name.Contains(name));

        Query.OrderBy(v => v.Name);

        Query.Skip((index - 1) * size)
             .Take(size);
    }
}