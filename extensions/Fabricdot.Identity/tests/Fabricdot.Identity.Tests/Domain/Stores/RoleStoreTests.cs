using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

namespace Fabricdot.Identity.Tests.Domain.Stores;

public class RoleStoreTests : RoleStoreTestBase
{
    [Fact]
    public async Task CreateAsync_GivenNull_ThrownException()
    {
        await RoleStore.Awaiting(v => v.CreateAsync(null, default))
                       .Should()
                       .ThrowAsync<ArgumentNullException>();
    }

    [AutoData]
    [Theory]
    public async Task CreateAsync_GivenRole_Correctly(Role role)
    {
        var res = await RoleStore.CreateAsync(role, default);
        var retrievalRole = await RoleRepository.GetByIdAsync(role.Id);

        res.Succeeded.Should().BeTrue();
        retrievalRole.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenNull_ThrownException()
    {
        await RoleStore.Awaiting(v => v.UpdateAsync(null, default))
                       .Should()
                       .ThrowAsync<ArgumentNullException>();
    }

    [AutoData]
    [Theory]
    public async Task UpdateAsync_GivenRole_Correctly(string description)
    {
        var role = await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId);
        role.Description = description;
        var res = await RoleStore.UpdateAsync(role, default);

        res.Succeeded.Should().BeTrue();
        role.Description.Should().Be(description);
    }

    [Fact]
    public async Task DeleteAsync_GivenNull_ThrownException()
    {
        await RoleStore.Awaiting(v => v.DeleteAsync(null, default))
                       .Should()
                       .ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAsync_GivenRole_Correctly()
    {
        var role = await RoleRepository.GetByIdAsync(FakeDataBuilder.RoleAuthorId);
        var res = await RoleStore.DeleteAsync(role, default);
        var retrievalRole = await RoleRepository.GetByIdAsync(role.Id);

        res.Succeeded.Should().BeTrue();
        retrievalRole.Should().BeNull();
    }

    [Fact]
    public async Task FindByIdAsync_GivenInput_ReturnCorrectly()
    {
        var roleId = FakeDataBuilder.RoleAuthorId;
        var role = await RoleStore.FindByIdAsync(roleId.ToString(), default);

        role.Should().NotBeNull();
        role.Id.Should().Be(roleId);
    }

    [Fact]
    public async Task FindByNameAsync_GivenInput_ReturnCorrectly()
    {
        var normalizedRoleName = LookupNormalizer.NormalizeName(FakeDataBuilder.RoleAuthor);
        var role = await RoleStore.FindByNameAsync(normalizedRoleName, default);

        role.Should().NotBeNull();
        role.NormalizedName.Should().Be(normalizedRoleName);
    }

    [AutoData]
    [Theory]
    public async Task GetRoleNameAsync_Should_Correctly(Role role)
    {
        var expected = role.Name;
        var actual = await RoleStore.GetRoleNameAsync(role, default);

        actual.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetNormalizedRoleNameAsync_Should_Correctly(Role role)
    {
        var expected = role.NormalizedName;
        var actual = await RoleStore.GetNormalizedRoleNameAsync(role, default);

        actual.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task GetRoleIdAsync_Should_Correctly(Role role)
    {
        var expected = role.Id.ToString();
        var actual = await RoleStore.GetRoleIdAsync(role, default);

        actual.Should().Be(expected);
    }

    [AutoData]
    [Theory]
    public async Task SetRoleNameAsync_Should_Correctly(Role role, string name)
    {
        await RoleStore.SetRoleNameAsync(role, name, default);

        role.Name.Should().Be(name);
    }

    [AutoData]
    [Theory]
    public async Task SetNormalizedRoleNameAsync_Should_Correctly(Role role, string name)
    {
        await RoleStore.SetNormalizedRoleNameAsync(role, name, default);

        role.NormalizedName.Should().Be(name);
    }
}