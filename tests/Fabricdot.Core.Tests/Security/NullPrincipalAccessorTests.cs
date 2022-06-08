using System;
using System.Security.Claims;
using Fabricdot.Core.Security;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.Security;

public class NullPrincipalAccessorTests
{
    [Fact]
    public void Principal_AlwaysReturnEmptyPrincpal()
    {
        var principal = new NullPrincipalAccessor().Principal;

        principal.Should().NotBeNull();
        principal.Identities.Should().BeEmpty();
    }

    [Fact]
    public void Change_GivenInput_ThrowException()
    {
        FluentActions.Invoking(() => new NullPrincipalAccessor().Change(new ClaimsPrincipal()))
                     .Should()
                     .ThrowExactly<NotSupportedException>();
    }
}