using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class RoleClaimStoreTests : RoleStoreTestBase
{
    private readonly IRoleClaimStore<Role> _roleClaimStore;

    public RoleClaimStoreTests()
    {
        _roleClaimStore = (IRoleClaimStore<Role>)RoleStore;
    }

    [Fact]
    public async Task AddClaimAsync_GivenNull_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            Task TestCode() => _roleClaimStore.AddClaimAsync(role, null, default);

            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        });
    }

    [Fact]
    public async Task AddClaimAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            var claim = new Claim("claimType2", "value2");
            await _roleClaimStore.AddClaimAsync(role, claim, default);

            Assert.Contains(role.Claims, v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenNull_ThrownException()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            Task TestCode() => _roleClaimStore.RemoveClaimAsync(role, null, default);

            await Assert.ThrowsAsync<ArgumentNullException>(TestCode);
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetDetailsByIdAsync(FakeDataBuilder.RoleAuthorId);
            var roleClaim = role.Claims.First();
            var claim = new Claim(roleClaim.ClaimType, roleClaim.ClaimValue);
            await _roleClaimStore.RemoveClaimAsync(role, claim, default);

            Assert.DoesNotContain(role.Claims, v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }
}