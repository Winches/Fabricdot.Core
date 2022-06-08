using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class UserClaimStoreTests : UserStoreTestBase
{
    private readonly IUserClaimStore<User> _userClaimStore;

    public UserClaimStoreTests()
    {
        _userClaimStore = (IUserClaimStore<User>)UserStore;
    }

    [Fact]
    public async Task AddClaimsAsync_GivenNullOrEmpty_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            Task TestCode1() => _userClaimStore.AddClaimsAsync(user, null, default);
            Task TestCode2() => _userClaimStore.AddClaimsAsync(user, Array.Empty<Claim>(), default);

            await Assert.ThrowsAsync<ArgumentNullException>(TestCode1);
            await Assert.ThrowsAsync<ArgumentException>(TestCode2);
        });
    }

    [Fact]
    public async Task AddClaimsAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var claim = new Claim("claimType1", "value1");
            await _userClaimStore.AddClaimsAsync(
                user,
                new[] { claim },
                default);

            Assert.Contains(user.Claims, v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenNullOrEmpty_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            Task TestCode1() => _userClaimStore.RemoveClaimsAsync(user, null, default);
            Task TestCode2() => _userClaimStore.RemoveClaimsAsync(user, Array.Empty<Claim>(), default);

            await Assert.ThrowsAsync<ArgumentNullException>(TestCode1);
            await Assert.ThrowsAsync<ArgumentException>(TestCode2);
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var userClaim = user.Claims.First();
            var claim = new Claim(userClaim.ClaimType, userClaim.ClaimValue);
            await _userClaimStore.RemoveClaimsAsync(
                user,
                new[] { claim },
                default);

            Assert.DoesNotContain(user.Claims, v => v.ClaimType == claim.Type && v.ClaimValue == claim.Value);
        });
    }

    [Fact]
    public async Task ReplaceClaimAsync_GivenNull_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var userClaim = user.Claims.First();
            var originClaim = new Claim(userClaim.ClaimType, userClaim.ClaimValue);
            var newClaim = new Claim(userClaim.ClaimType, "newValue");
            Task TestCode1() => _userClaimStore.ReplaceClaimAsync(user, originClaim, null, default);
            Task TestCode2() => _userClaimStore.ReplaceClaimAsync(user, null, newClaim, default);

            await Assert.ThrowsAsync<ArgumentNullException>(TestCode1);
            await Assert.ThrowsAsync<ArgumentNullException>(TestCode2);
        });
    }

    [Fact]
    public async Task ReplaceClaimAsync_GivenClaims_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var userClaim = user.Claims.First();
            var originClaim = new Claim(userClaim.ClaimType, userClaim.ClaimValue);
            var newClaim = new Claim(userClaim.ClaimType, "newValue");
            await _userClaimStore.ReplaceClaimAsync(
                user,
                originClaim,
                newClaim,
                default);

            Assert.DoesNotContain(user.Claims, v => v.ClaimType == originClaim.Type && v.ClaimValue == originClaim.Value);
            Assert.Contains(user.Claims, v => v.ClaimType == newClaim.Type && v.ClaimValue == newClaim.Value);
        });
    }

    [Fact]
    public async Task GetClaimsAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = await UserRepository.GetDetailsByIdAsync(FakeDataBuilder.UserAndersId);
            var claims = await _userClaimStore.GetClaimsAsync(user, default);

            Assert.Contains(claims, v => user.Claims.Any(o => o.ClaimType == v.Type && o.ClaimValue == v.Value));
        });
    }

    [Fact]
    public async Task GetUsersForClaimAsync_ReturnCorrectly()
    {
        var users = await _userClaimStore.GetUsersForClaimAsync(FakeDataBuilder.ClaimOfAnders, default);

        Assert.Contains(users, v => v.Id == FakeDataBuilder.UserAndersId);
    }
}