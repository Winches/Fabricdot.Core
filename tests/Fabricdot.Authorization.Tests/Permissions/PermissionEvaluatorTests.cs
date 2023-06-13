using System.Security.Claims;
using Fabricdot.Authorization.Permissions;
using Fabricdot.Core.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionEvaluatorTests : AuthorizationTestBase
{
    protected IPermissionEvaluator PermissionEvaluator { get; }

    public PermissionEvaluatorTests()
    {
        PermissionEvaluator = ServiceProvider.GetRequiredService<IPermissionEvaluator>();
    }

    public static IEnumerable<object?[]> GetInvalidInput()
    {
        yield return new object?[] { null, new[] { new PermissionName("name") } };
        yield return new object?[] { new ClaimsPrincipal(), null };
        yield return new object?[] { new ClaimsPrincipal(), Array.Empty<PermissionName>() };
    }

    [MemberData(nameof(GetInvalidInput))]
    [Theory]
    public async Task EvaluateAsync_GivenInvalidInput_Throw(
        ClaimsPrincipal principal,
        IEnumerable<PermissionName> permissions)
    {
        async Task TestCode() => await PermissionEvaluator.EvaluateAsync(principal, permissions);

        await Awaiting(TestCode).Should().ThrowAsync<ArgumentException>();
    }

    [AutoData]
    [Theory]
    public async Task EvaluateAsync_GivenUndefinedPermission_Throw(ClaimsPrincipal principal, PermissionName[] permissions)
    {
        async Task TestCode() => await PermissionEvaluator.EvaluateAsync(principal, permissions);

        await Awaiting(TestCode).Should().ThrowAsync<PermissionNotDefinedException>();
    }

    [Fact]
    public async Task EvaluateAsync_GivenInput_ReturnCorrectly()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] {
            new Claim(SharedClaimTypes.NameIdentifier, Create<string>())
        }));
        var grantResults = await PermissionEvaluator.EvaluateAsync(principal, Permissions);

        grantResults.Should().HaveSameCount(Permissions);
        grantResults.Should().AllSatisfy(result =>
        {
            var expected = GrantedPermissions.Contains(result.Object);
            Permissions.Should().Contain(result.Object);
            result.IsGranted.Should().Be(expected);
        });
    }

    [Fact]
    public async Task EvaluateAsync_GivenSuperuser_ReturnCorrectly()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { Superuser }));
        var grantResults = await PermissionEvaluator.EvaluateAsync(principal, Permissions);

        grantResults.Should().HaveSameCount(Permissions);
        grantResults.Should().OnlyContain(v => v.IsGranted);
    }

    [Fact]
    public async Task EvaluateAsync_WhenOneSubjectSatisfied_ReturnCorrectly()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] {
            Superrole,
            new Claim(SharedClaimTypes.Role, Create<string>())
        }));
        var grantResults = await PermissionEvaluator.EvaluateAsync(principal, GrantedPermissions);

        grantResults.Should().AllSatisfy(result => result.IsGranted.Should().BeTrue());
    }
}
