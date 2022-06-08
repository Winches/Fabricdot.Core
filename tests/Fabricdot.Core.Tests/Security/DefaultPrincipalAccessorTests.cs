using System.Security.Claims;
using System.Threading;
using Fabricdot.Core.Security;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Security;

public class DefaultPrincipalAccessorTests
{
    [Fact]
    public void Change_GivenInput_SetPrincpalWithScope()
    {
        var principalAccessor = new DefaultPrincipalAccessor();
        var principal = new ClaimsPrincipal();
        using (var scope1 = principalAccessor.Change(principal))
        {
            scope1.Should().NotBeNull();
            principalAccessor.Principal.Should().BeSameAs(principal);

            var innerPrincipal = new ClaimsPrincipal();
            using (var scope2 = principalAccessor.Change(innerPrincipal))
            {
                principalAccessor.Principal.Should().BeSameAs(innerPrincipal);
            }
            principalAccessor.Principal.Should().BeSameAs(principal);
        }
        principalAccessor.Principal.Should().BeNull();
    }

    [Fact]
    public void Principal_WhenPrincipalIsNull_ReturnThreadPrincipal()
    {
        var principalAccessor = new DefaultPrincipalAccessor();
        var principal = new ClaimsPrincipal();

        principalAccessor.Principal.Should().BeNull();
        Thread.CurrentPrincipal = principal;
        principalAccessor.Principal.Should().BeSameAs(principal);
    }
}