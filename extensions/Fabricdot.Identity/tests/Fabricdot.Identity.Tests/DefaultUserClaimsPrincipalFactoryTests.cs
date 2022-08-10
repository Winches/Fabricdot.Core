using System.Security.Claims;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests;

public class DefaultUserClaimsPrincipalFactoryTests : IdentityTestBase
{
    private readonly IUserClaimsPrincipalFactory<User> _factory;
    private readonly IUserStore<User> _userStore;

    public DefaultUserClaimsPrincipalFactoryTests()
    {
        _factory = ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<User>>();
        _userStore = ServiceProvider.GetRequiredService<IUserStore<User>>();
    }

    [Fact]
    public async Task CreateAsync_GivenUserDetails_CreatePrincipal()
    {
        await UseUowAsync(async () =>
        {
            var userId = FakeDataBuilder.UserAndersId.ToString();
            var user = await _userStore.FindByIdAsync(userId, default);
            var userRoles = await ((IUserRoleStore<User>)_userStore).GetRolesAsync(user, default);
            var claimsPrincipal = await _factory.CreateAsync(user);

            claimsPrincipal.Should().NotBeNull();
            claimsPrincipal.Should()
                           .HaveSingleClaim(ClaimTypes.GivenName, user.GivenName).And
                           .HaveSingleClaim(ClaimTypes.Surname, user.Surname).And
                           .HaveSingleClaim(ClaimTypes.Email, user.Email).And
                           .HaveSameClaimValues(ClaimTypes.Role, userRoles);
        });
    }
}