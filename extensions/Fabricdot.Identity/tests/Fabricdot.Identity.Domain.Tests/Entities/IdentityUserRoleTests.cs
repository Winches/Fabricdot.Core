using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Testing.AutoFixture;

namespace Fabricdot.Identity.Domain.Tests.Entities;

public class IdentityUserRoleTests : TestFor<IdentityUserRole>
{
    [Fact]
    public void Equality_Should_Correctly()
    {
        var sut = typeof(IdentityUserRole);

        Create<EqualityAssertion>().Verify(sut);
    }
}