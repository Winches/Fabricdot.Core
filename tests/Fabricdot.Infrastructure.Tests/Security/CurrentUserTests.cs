using System.Security.Claims;
using Fabricdot.Core.Security;
using Fabricdot.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Security;

public class CurrentUserTests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly ICurrentUser _currentUser;
    public static ClaimsPrincipal? ClaimsPrincipal { get; set; }

    public CurrentUserTests()
    {
        _currentUser = ServiceProvider.GetRequiredService<ICurrentUser>();
    }

    public static IEnumerable<object?[]> GetClaimPrincipals()
    {
        yield return new object?[] { null };
        yield return new object?[] { CreateClaimsPrincipal() };
        yield return new object?[]
        {
            CreateClaimsPrincipal(new Claim(SharedClaimTypes.NameIdentifier, "1"))
        };
        yield return new object[]
        {
            CreateClaimsPrincipal(new Claim(SharedClaimTypes.Name, "Allen"))
        };

        yield return new object[]
        {
            CreateClaimsPrincipal(
                new Claim(SharedClaimTypes.Role, "Admin"),
                new Claim(SharedClaimTypes.Role, "Employee"))
        };
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void Id_Should_ReturnNameIdentifier(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindFirst(SharedClaimTypes.NameIdentifier)?.Value;

        _currentUser.Id.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void UserName_Should_ReturnName(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindFirst(SharedClaimTypes.Name)?.Value;

        _currentUser.UserName.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void Roles_Should_ReturnAllRoles(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindAll(SharedClaimTypes.Role).Select(v => v.Value).ToArray() ??
                       Array.Empty<string>();

        _currentUser.Roles.Should().BeEquivalentTo(expected);
    }

    [InlineData("")]
    [InlineAutoData]
    [Theory]
    public void IsAuthenticated_Should_ReturnCorrectly(string id)
    {
        var expected = !id.IsNullOrEmpty();
        ClaimsPrincipal = CreateClaimsPrincipal(new Claim(SharedClaimTypes.NameIdentifier, id));

        _currentUser.IsAuthenticated.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public void IsInRole_GivenExistRole_ReturnTrue(string role)
    {
        ClaimsPrincipal = CreateClaimsPrincipal(new Claim(SharedClaimTypes.Role, role));

        _currentUser.IsInRole(role).Should().BeTrue();
    }

    [Fact]
    public void IsInRole_GivenNull_ReturnFalse()
    {
        ClaimsPrincipal = Create<ClaimsPrincipal>();

        _currentUser.IsInRole(null!).Should().BeFalse();
    }

    [Fact]
    public void GetAllClaims_WhenPrincipalNotNull_ReturnCorrectly()
    {
        var expected = Create<Claim[]>();
        ClaimsPrincipal = CreateClaimsPrincipal(expected);

        _currentUser.GetAllClaims().Should().BeEquivalentTo(expected, opts => opts.Excluding(v => v.Subject));
    }

    [Fact]
    public void GetAllClaims_WhenPrincipalIsNull_ReturnEmpty()
    {
        ClaimsPrincipal = null;

        _currentUser.GetAllClaims().Should().BeEmpty();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var mock = Mock<IPrincipalAccessor>();
        mock.SetupGet(v => v.Principal).Returns(() => ClaimsPrincipal);
        serviceCollection.AddSingleton(_ => mock.Object);
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(params Claim[] claims)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(claims ?? Array.Empty<Claim>(), "Bearer"));
    }
}
