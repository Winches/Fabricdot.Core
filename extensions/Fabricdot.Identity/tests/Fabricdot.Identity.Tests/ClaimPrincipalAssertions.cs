using System.Security.Claims;
using FluentAssertions.Primitives;

namespace Fabricdot.Identity.Tests;

public class ClaimPrincipalAssertions : ObjectAssertions
{
    private readonly ClaimsPrincipal _claimsPrincipal;

    public ClaimPrincipalAssertions(ClaimsPrincipal claimsPrincipal) : base(claimsPrincipal)
    {
        _claimsPrincipal = claimsPrincipal;
    }

    [CustomAssertion]
    public AndConstraint<ClaimPrincipalAssertions> HaveSingleClaim(
        string claimType,
        string claimValue,
        string because = "",
        params object[] becauseArgs)
    {
        _claimsPrincipal.Claims.Should()
                               .ContainSingle(v => v.Type == claimType && v.Value == claimValue, because, becauseArgs);
        return new AndConstraint<ClaimPrincipalAssertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<ClaimPrincipalAssertions> HaveSameClaimValues(
    string claimType,
    IEnumerable<string> claimValues,
    string because = "",
    params object[] becauseArgs)
    {
        _claimsPrincipal.FindAll(claimType)
                        .Select(v => v.Value)
                        .Should()
                        .BeEquivalentTo(claimValues, because, becauseArgs);
        return new AndConstraint<ClaimPrincipalAssertions>(this);
    }
}