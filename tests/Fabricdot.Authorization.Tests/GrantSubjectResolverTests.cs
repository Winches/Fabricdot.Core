using System.Security.Claims;

namespace Fabricdot.Authorization.Tests;

public class GrantSubjectResolverTests : TestFor<GrantSubjectResolver>
{
    [Fact]
    public async Task ResolveAsync_GivenInput_ReturnCorrectly()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role,Create<string>()),
            new Claim(ClaimTypes.NameIdentifier,Create<string>()),
            Create<Claim>(),
        };

        var expected = claims.Select(v => v.Type switch
        {
            ClaimTypes.NameIdentifier => GrantSubject.User(v.Value),
            ClaimTypes.Role => GrantSubject.Role(v.Value),
            _ => (GrantSubject?)null
        })
            .Where(v => v is not null);
        var pricinpal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var grantSubjects = await Sut.ResolveAsync(pricinpal);

        grantSubjects.Should().BeEquivalentTo(expected);
    }
}