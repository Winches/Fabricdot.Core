using System.Security.Claims;
using Fabricdot.Core.Security;

namespace Fabricdot.Core.Tests.Security;

public class NullPrincipalAccessorTests : TestFor<NullPrincipalAccessor>
{
    [Fact]
    public void Principal_AlwaysReturnEmptyPrincpal()
    {
        var principal = Sut.Principal;

        principal.Should().NotBeNull();
        principal!.Identities.Should().BeEmpty();
    }

    [Fact]
    public void Change_GivenInput_ThrowException()
    {
        Invoking(() => Sut.Change(Create<ClaimsPrincipal>()))
                     .Should()
                     .ThrowExactly<NotSupportedException>();
    }
}
