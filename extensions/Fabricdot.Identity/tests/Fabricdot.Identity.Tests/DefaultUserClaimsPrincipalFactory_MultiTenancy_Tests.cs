using Fabricdot.Domain.SharedKernel;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.Data.Filters;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests;

public class DefaultUserClaimsPrincipalFactory_MultiTenancy_Tests : IdentityTestBase
{
    private readonly IUserClaimsPrincipalFactory<User> _factory;
    private readonly IUserStore<User> _userStore;
    private readonly IDataFilter _dataFilter;

    public DefaultUserClaimsPrincipalFactory_MultiTenancy_Tests()
    {
        _factory = ServiceProvider.GetRequiredService<IUserClaimsPrincipalFactory<User>>();
        _userStore = ServiceProvider.GetRequiredService<IUserStore<User>>();
        _dataFilter = ServiceProvider.GetRequiredService<IDataFilter>();
    }

    [Fact]
    public async Task CreateAsync_WithMultiTenancy_SetTenantId()
    {
        using var scope = _dataFilter.Disable<IMultiTenant>();
        await UseUowAsync(async () =>
        {
            var userId = FakeDataBuilder.TenantUserId.ToString();
            var user = await _userStore.FindByIdAsync(userId, default);
            var claimsPrincipal = await _factory.CreateAsync(user);

            claimsPrincipal.Should().NotBeNull();
            claimsPrincipal.Should().HaveSingleClaim(TenantClaimTypes.TenantId, user.TenantId.ToString()!);
        });
    }
}
