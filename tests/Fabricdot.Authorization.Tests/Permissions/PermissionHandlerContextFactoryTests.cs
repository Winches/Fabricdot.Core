using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions;

public class PermissionHandlerContextFactoryTests
{
    protected IPermissionHandlerContextFactory PermissionHandlerContextFactory { get; }

    public PermissionHandlerContextFactoryTests()
    {
        var mockSubjectResolver = new Mock<IGrantSubjectResolver>();
        mockSubjectResolver.Setup(v => v.ResolveAsync(It.IsAny<ClaimsPrincipal>(), default))
                           .ReturnsAsync((
                               ClaimsPrincipal principal,
                               CancellationToken _) => principal.Claims.Select(v => (GrantSubject)v).ToList());
        PermissionHandlerContextFactory = new PermissionHandlerContextFactory(mockSubjectResolver.Object);
    }

    public static IEnumerable<object[]> GetInvalidInput()
    {
        yield return new object[] { null, new[] { new PermissionName("name") } };
        yield return new object[] { new ClaimsPrincipal(), null };
        yield return new object[] { new ClaimsPrincipal(), Array.Empty<PermissionName>() };
    }

    [Fact]
    public async Task CreateAsync_GivenNullPrincipal_Throw()
    {
        async Task testCode() => await PermissionHandlerContextFactory.CreateAsync(
            null,
            new[] { new PermissionName("name1") });

        await FluentActions.Awaiting(testCode)
                           .Should()
                           .ThrowAsync<ArgumentNullException>();
    }

    [MemberData(nameof(GetInvalidInput))]
    [Theory]
    public async Task CreateAsync_GivenInvalidInput_Throw(
        ClaimsPrincipal principal,
        IEnumerable<PermissionName> permissions)
    {
        async Task testCode() => await PermissionHandlerContextFactory.CreateAsync(
            principal,
            permissions);

        await FluentActions.Awaiting(testCode)
                           .Should()
                           .ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task CreateAsync_GivenInput_ReturnCorrectly()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role,"role1"),
            new Claim(ClaimTypes.NameIdentifier,"1"),
            new Claim(ClaimTypes.Name,"Jacky"),
        };
        var expectedClaims = claims.Select(v => (GrantSubject)v).ToList();
        var pricinpal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var permissions = new List<PermissionName>
        {
            new PermissionName("name1")
        };
        var context = await PermissionHandlerContextFactory.CreateAsync(pricinpal, permissions);

        context.Should().NotBeNull();
        context.Subjects.Should().BeEquivalentTo(expectedClaims);
        context.Permissions.Should().BeEquivalentTo(permissions);
        permissions.Clear();
        context.Permissions.Should().NotBeNullOrEmpty();
    }
}