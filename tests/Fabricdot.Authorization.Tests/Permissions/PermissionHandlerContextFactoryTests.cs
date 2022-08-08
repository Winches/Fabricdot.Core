using System.Security.Claims;
using Fabricdot.Authorization.Permissions;
using Moq;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionHandlerContextFactoryTests : TestFor<PermissionHandlerContextFactory>
{
    public PermissionHandlerContextFactoryTests()
    {
        var mockSubjectResolver = InjectMock<IGrantSubjectResolver>();
        mockSubjectResolver.Setup(v => v.ResolveAsync(It.IsAny<ClaimsPrincipal>(), default))
                           .ReturnsAsync((
                               ClaimsPrincipal principal,
                               CancellationToken _) => principal.Claims.Select(v => (GrantSubject)v).ToList());
    }

    public static IEnumerable<object[]> GetInvalidInput()
    {
        yield return new object[] { null, new[] { new PermissionName("name") } };
        yield return new object[] { new ClaimsPrincipal(), null };
        yield return new object[] { new ClaimsPrincipal(), Array.Empty<PermissionName>() };
    }

    [MemberData(nameof(GetInvalidInput))]
    [Theory]
    public async Task CreateAsync_GivenInvalidInput_Throw(
        ClaimsPrincipal principal,
        IEnumerable<PermissionName> permissions)
    {
        await Awaiting(() => Sut.CreateAsync(principal, permissions))
                           .Should()
                           .ThrowAsync<ArgumentException>();
        // TODO:Simplify
    }

    [AutoData]
    [Theory]
    public async Task CreateAsync_GivenInput_ReturnCorrectly(List<PermissionName> permissions)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role,Create<string>()),
            new Claim(ClaimTypes.NameIdentifier,Create<string>()),
            new Claim(ClaimTypes.Name,Create<string>()),
        };
        var expectedClaims = claims.Select(v => (GrantSubject)v).ToList();
        var pricinpal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var context = await Sut.CreateAsync(pricinpal, permissions);

        context.Should().NotBeNull();
        context.Subjects.Should().BeEquivalentTo(expectedClaims);
        context.Permissions.Should().BeEquivalentTo(permissions);
        permissions.Clear();
        context.Permissions.Should().NotBeNullOrEmpty();
    }
}