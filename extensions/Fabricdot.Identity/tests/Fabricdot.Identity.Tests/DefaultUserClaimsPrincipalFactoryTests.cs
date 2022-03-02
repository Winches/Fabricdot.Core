using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Identity.Tests
{
    public class DefaultUserClaimsPrincipalFactoryTests : IdentityTestBase
    {
        private readonly IUserClaimsPrincipalFactory<User> _factory;
        private readonly IUserStore<User> _userStore;

        public DefaultUserClaimsPrincipalFactoryTests()
        {
            _factory = ScopeServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<User>>();
            _userStore = ScopeServiceProvider.GetRequiredService<IUserStore<User>>();
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

                var givenName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);
                var surname = claimsPrincipal.FindFirstValue(ClaimTypes.Surname);
                var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
                var phone = claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);
                var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(v => v.Value);

                Assert.NotNull(claimsPrincipal);
                Assert.Equal(user.GivenName, givenName);
                Assert.Equal(user.Surname, surname);
                Assert.Equal(user.Email, email);
                Assert.Equal(user.PhoneNumber, phone);
                Assert.Equal(userRoles, roles);
            });
        }
    }
}