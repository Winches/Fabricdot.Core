using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Fabricdot.Core.Security;
using Fabricdot.Infrastructure.Security;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Security;

public class CurrentUserTests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly ICurrentUser _currentUser;
    public static ClaimsPrincipal ClaimsPrincipal { get; set; }

    public CurrentUserTests()
    {
        _currentUser = ServiceProvider.GetRequiredService<ICurrentUser>();
    }

    public static IEnumerable<object[]> GetClaimPrincipals()
    {
        yield return new object[] { null };
        yield return new object[] { CreateClaimsPrincipal() };
        yield return new object[]
        {
            CreateClaimsPrincipal(new Claim(ClaimTypes.NameIdentifier, "1"))
        };
        yield return new object[]
        {
            CreateClaimsPrincipal(new Claim(ClaimTypes.Name, "Allen"))
        };

        yield return new object[]
        {
            CreateClaimsPrincipal(new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "Allen"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Employee"))
        };
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void Id_ReturnNameIdentifierClaim(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var actual = _currentUser.Id;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void UserName_ReturnNameClaim(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindFirst(ClaimTypes.Name)?.Value;
        var actual = _currentUser.UserName;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetClaimPrincipals))]
    public void Roles_ReturnAllRoleClaims(ClaimsPrincipal claimsPrincipal)
    {
        ClaimsPrincipal = claimsPrincipal;
        var expected = claimsPrincipal?.FindAll(ClaimTypes.Role).Select(v => v.Value).ToArray() ??
                       Array.Empty<string>();
        var actual = _currentUser.Roles;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void IsInRole_GivenExistRole_ReturnTrue()
    {
        const string role = "Admin";
        ClaimsPrincipal = CreateClaimsPrincipal(new Claim(ClaimTypes.Role, role));
        var condition = _currentUser.IsInRole(role);
        Assert.True(condition);
    }

    [Fact]
    public void IsInRole_GivenNull_ReturnFalse()
    {
        ClaimsPrincipal = CreateClaimsPrincipal();
        var condition = _currentUser.IsInRole(null);
        Assert.False(condition);
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var mock = new Mock<IPrincipalAccessor>();
        mock.SetupGet(v => v.Principal).Returns(() => ClaimsPrincipal);
        serviceCollection.AddScoped(_ => mock.Object);
        //serviceCollection.AddScoped<ICurrentUser, CurrentUser>();
    }

    protected IDisposable ChangeCurrentPrincipal(ClaimsPrincipal claimsPrincipal)
    {
        return ServiceScope.ServiceProvider.GetRequiredService<IPrincipalAccessor>().Change(claimsPrincipal);
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(params Claim[] claims)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var claimsIdentity = new ClaimsIdentity(claims ?? Array.Empty<Claim>(), "Bearer");
        claimsPrincipal.AddIdentity(claimsIdentity);
        return claimsPrincipal;
    }
}