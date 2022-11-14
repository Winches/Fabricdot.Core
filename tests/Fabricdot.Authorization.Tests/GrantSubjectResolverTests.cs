using System.Security.Claims;
using Fabricdot.Core.Security;

namespace Fabricdot.Authorization.Tests;

public class GrantSubjectResolverTests : TestFor<GrantSubjectResolver>
{
    [Fact]
    public async Task ResolveAsync_GivenInput_ReturnCorrectly()
    {
        var claims = new[]
        {
            new Claim(SharedClaimTypes.Role,Create<string>()),
            new Claim(SharedClaimTypes.NameIdentifier,Create<string>()),
            Create<Claim>(),
        };

        var expected = claims.Select(v => v.Type switch
        {
            var x when x == SharedClaimTypes.NameIdentifier => GrantSubject.User(v.Value),
            var y when y == SharedClaimTypes.Role => GrantSubject.Role(v.Value),
            _ => (GrantSubject?)null
        })
            .Where(v => v is not null);
        var pricinpal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var grantSubjects = await Sut.ResolveAsync(pricinpal);

        grantSubjects.Should().BeEquivalentTo(expected);
    }
}