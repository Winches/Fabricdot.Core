using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Identity.Domain.Entities.UserAggregate;

public class IdentityUserRole : ValueObject
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public IdentityUserRole(Guid roleId)
    {
        RoleId = roleId;
    }

    private IdentityUserRole()
    {
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return UserId;
        yield return RoleId;
    }
}