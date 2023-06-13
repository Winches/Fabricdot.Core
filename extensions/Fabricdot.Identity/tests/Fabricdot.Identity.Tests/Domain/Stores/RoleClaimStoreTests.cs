using System.Security.Claims;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

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
            var role = await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId);

            await _roleClaimStore.Awaiting(v => v.AddClaimAsync(role!, null, default))
                                 .Should()
                                 .ThrowAsync<ArgumentNullException>();
        });
    }

    [AutoMockData]
    [Theory]
    public async Task AddClaimAsync_GivenClaim_Correctly(Claim claim)
    {
        await UseUowAsync(async () =>
        {
            var role = (await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId))!;
            await _roleClaimStore.AddClaimAsync(role, claim, default);

            role.Claims.Should().ContainSingle(v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenNull_ThrownException()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId);

            await _roleClaimStore.Awaiting(v => v.RemoveClaimAsync(role!, null, default))
                                 .Should()
                                 .ThrowAsync<ArgumentNullException>();
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var role = await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId);
            var roleClaim = role!.Claims.First();
            var claim = new Claim(roleClaim.ClaimType, roleClaim.ClaimValue!);
            await _roleClaimStore.RemoveClaimAsync(role, claim, default);

            role.Claims.Should().NotContain(v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }
}
