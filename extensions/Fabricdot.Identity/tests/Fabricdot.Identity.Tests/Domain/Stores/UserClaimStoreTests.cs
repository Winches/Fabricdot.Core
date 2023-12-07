using System.Security.Claims;
using Fabricdot.Identity.Domain.Entities.UserAggregate;
using Fabricdot.Identity.Domain.SharedKernel;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.AspNetCore.Identity;

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
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            Task TestCode1() => _userClaimStore.AddClaimsAsync(user, null!, default);
            Task TestCode2() => _userClaimStore.AddClaimsAsync(user, Array.Empty<Claim>(), default);

            await Awaiting(TestCode1).Should().ThrowAsync<ArgumentNullException>();
            await Awaiting(TestCode2).Should().ThrowAsync<ArgumentException>();
        });
    }

    [AutoMockData]
    [Theory]
    public async Task AddClaimsAsync_GivenClaim_Correctly(Claim[] claims)
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            await _userClaimStore.AddClaimsAsync(
                user,
                claims,
                default);

            user.Claims.Should().Contain(v => claims.Any(o => o.Type == v.ClaimType && o.Value == v.ClaimValue));
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenNullOrEmpty_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            Task TestCode1() => _userClaimStore.RemoveClaimsAsync(user, null!, default);
            Task TestCode2() => _userClaimStore.RemoveClaimsAsync(user, Array.Empty<Claim>(), default);

            await Awaiting(TestCode1).Should().ThrowAsync<ArgumentNullException>();
            await Awaiting(TestCode2).Should().ThrowAsync<ArgumentException>();
        });
    }

    [Fact]
    public async Task RemoveClaimsAsync_GivenClaim_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            var userClaim = user.Claims.First();
            await _userClaimStore.RemoveClaimsAsync(
                user,
                new[] { new Claim(userClaim.ClaimType, userClaim.ClaimValue!) },
                default);

            user.Claims.Should().NotContain(userClaim);
        });
    }

    [Fact]
    public async Task ReplaceClaimAsync_GivenNull_ThrowException()
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            var userClaim = user.Claims.First();
            var originClaim = new Claim(userClaim.ClaimType, userClaim.ClaimValue!);
            var newClaim = new Claim(userClaim.ClaimType, Create<string>());
            Task TestCode1() => _userClaimStore.ReplaceClaimAsync(user, originClaim, null!, default);
            Task TestCode2() => _userClaimStore.ReplaceClaimAsync(user, null!, newClaim, default);

            await Awaiting(TestCode1).Should().ThrowAsync<ArgumentNullException>();
            await Awaiting(TestCode2).Should().ThrowAsync<ArgumentNullException>();
        });
    }

    [Fact]
    public async Task ReplaceClaimAsync_GivenClaims_Correctly()
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            var originClaim = user.Claims.First().ToClaim();
            var newClaim = new Claim(originClaim.Type, Create<string>());
            await _userClaimStore.ReplaceClaimAsync(
                user,
                originClaim,
                newClaim,
                default);

            user.Claims.Should()
                       .NotContain(v => v.ClaimType == originClaim.Type && v.ClaimValue == originClaim.Value).And
                       .Contain(v => v.ClaimType == newClaim.Type && v.ClaimValue == newClaim.Value);
        });
    }

    [Fact]
    public async Task GetClaimsAsync_ReturnCorrectly()
    {
        await UseUowAsync(async () =>
        {
            var user = (await UserRepository.GetByIdAsync(FakeDataBuilder.UserAndersId))!;
            var claims = await _userClaimStore.GetClaimsAsync(user, default);

            claims.Should().BeEquivalentTo(user.Claims, opts => opts.ComparingByMembers<IdentityUserClaim>().ExcludingMissingMembers());
        });
    }

    [Fact]
    public async Task GetUsersForClaimAsync_ReturnCorrectly()
    {
        var users = await _userClaimStore.GetUsersForClaimAsync(FakeDataBuilder.ClaimOfAnders, default);

        users.Should().Contain(v => v.Id == FakeDataBuilder.UserAndersId);
    }
}
