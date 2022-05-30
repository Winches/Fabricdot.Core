using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Authorization.Permissions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Authorization.Tests.Permissions
{
    public class PermissionEvaluatorTests : AuthorizationTestBase
    {
        protected IPermissionEvaluator PermissionEvaluator { get; }

        public PermissionEvaluatorTests()
        {
            PermissionEvaluator = ServiceProvider.GetRequiredService<IPermissionEvaluator>();
        }

        public static IEnumerable<object[]> GetInvalidInput()
        {
            yield return new object[] { null, new[] { new PermissionName("name") } };
            yield return new object[] { new ClaimsPrincipal(), null };
            yield return new object[] { new ClaimsPrincipal(), Array.Empty<PermissionName>() };
        }

        [MemberData(nameof(GetInvalidInput))]
        [Theory]
        public async Task EvaluateAsync_GivenInvalidInput_Throw(
            ClaimsPrincipal principal,
            IEnumerable<PermissionName> permissions)
        {
            async Task testCode() => await PermissionEvaluator.EvaluateAsync(principal, permissions);

            await FluentActions.Awaiting(testCode)
                               .Should()
                               .ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task EvaluateAsync_GivenUndefinedPermission_Throw()
        {
            async Task testCode() => await PermissionEvaluator.EvaluateAsync(
                new ClaimsPrincipal(),
                new[] { new PermissionName("Undefined") });

            await FluentActions.Awaiting(testCode)
                               .Should()
                               .ThrowAsync<PermissionNotDefinedException>();
        }

        [Fact]
        public async Task EvaluateAsync_GivenInput_ReturnCorrectly()
        {
            var permissions = GrantedPermissions.Union(UngrantedPermissions);
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));
            var grantResults = await PermissionEvaluator.EvaluateAsync(principal, permissions);

            grantResults.Should().HaveCount(permissions.Count());
            grantResults.Should().AllSatisfy(result =>
            {
                var expected = GrantedPermissions.Contains(result.Object);
                permissions.Should().Contain(result.Object);
                result.IsGranted.Should().Be(expected);
            });
        }

        [Fact]
        public async Task EvaluateAsync_GivenSuperuser_ReturnCorrectly()
        {
            var permissions = GrantedPermissions.Union(UngrantedPermissions);
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { Superuser }));
            var grantResults = await PermissionEvaluator.EvaluateAsync(principal, permissions);

            grantResults.Should().HaveCount(permissions.Count());
            grantResults.Should().OnlyContain(v => v.IsGranted);
        }

        [Fact]
        public async Task EvaluateAsync_WhenOneSubjectSatisfied_ReturnCorrectly()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { Superrole, new Claim(ClaimTypes.Role, "role1") }));
            var grantResults = await PermissionEvaluator.EvaluateAsync(principal, GrantedPermissions);

            grantResults.Should().AllSatisfy(result => result.IsGranted.Should().BeTrue());
        }
    }
}