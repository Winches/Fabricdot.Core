using System.Security.Claims;
using Fabricdot.Core.Security;

namespace Fabricdot.Core.Tests.Security;

public class DefaultPrincipalAccessorTests : TestFor<DefaultPrincipalAccessor>
{
    [AutoData]
    [Theory]
    public void Change_GivenInput_SetPrincpalWithScope(
        ClaimsPrincipal outerPrincipal,
        ClaimsPrincipal innerPrincipal)
    {
        using (var scope1 = Sut.Change(outerPrincipal))
        {
            scope1.Should().NotBeNull();
            Sut.Principal.Should().BeSameAs(outerPrincipal);

            using (var scope2 = Sut.Change(innerPrincipal))
            {
                Sut.Principal.Should().BeSameAs(innerPrincipal);
            }
            Sut.Principal.Should().BeSameAs(outerPrincipal);
        }
        Sut.Principal.Should().BeNull();
    }

    [Fact]
    public void Principal_WhenPrincipalIsNull_ReturnThreadPrincipal()
    {
        var principal = Create<ClaimsPrincipal>();

        Sut.Principal.Should().BeNull();
        Thread.CurrentPrincipal = principal;
        Sut.Principal.Should().BeSameAs(principal);
    }
}