using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Authorization.Tests;

public class GrantSubjectResolverTests
{
    protected IGrantSubjectResolver GrantSubjectResolver { get; } = new GrantSubjectResolver();

    [Fact]
    public async Task ResolveAsync_GivenInput_ReturnCorrectly()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role,"role1"),
            new Claim(ClaimTypes.NameIdentifier,"1"),
            new Claim(ClaimTypes.Name,"Jacky"),
        };
        var expectedClaims = claims.Select(v => v.Type switch
        {
            ClaimTypes.NameIdentifier => new GrantSubject(GrantTypes.User, v.Value),
            ClaimTypes.Role => new GrantSubject(GrantTypes.Role, v.Value),
            _ => (GrantSubject?)null
        })
            .Where(v => v is not null);
        var pricinpal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var grantSubjects = await GrantSubjectResolver.ResolveAsync(pricinpal);

        grantSubjects.Should().BeEquivalentTo(expectedClaims);
    }
}