using System.Security.Claims;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Identity.Domain.Repositories;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

[Dependency(ServiceLifetime.Transient)]
public class FakeDataBuilder
{
    public static readonly string UserAnders = "Anders";
    public static readonly Guid UserAndersId = new("edbaafdb-6665-4941-9fbf-ca1140fa80f0");
    public static readonly string RoleNameOfAnders = "role1";
    public static readonly Guid RoleIdOfAnders = new("6ca5767c-c692-4479-af60-765442aacb6d");
    public static readonly Claim ClaimOfAnders = new("claimtype1", "value1");
    public static readonly UserLoginInfo LoginOfAnders = new("microsoft", "1", "microsoft login");

    public static readonly string RoleAuthor = "Author";
    public static readonly Guid RoleAuthorId = new("2fb3078e-9bd0-4521-9e97-bd4c5b669ca7");

    public static readonly Guid TenantId = new("b87393db-1784-4dc8-aa45-3370fb04b36b");
    public static readonly Guid TenantUserId = new("284f0139-ab9c-4ecc-a44a-d99e3f6faa80");
    private readonly IFixture _fixture;
    private readonly IUserRepository<User> _userRepository;
    private readonly IRoleRepository<Role> _roleRepository;

    public FakeDataBuilder(
        IFixture fixture,
        IUserRepository<User> userRepository,
        IRoleRepository<Role> roleRepository)
    {
        _fixture = fixture;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task BuildAsync()
    {
        await AddRolesAsync();
        await AddUsersAsync();
    }

    [UnitOfWork]
    protected virtual async Task AddUsersAsync()
    {
        var users = new[]
        {
            new User(UserAndersId, UserAnders, "qwe1@banana.com")
            {
                GivenName = "Anders",
                Surname = "Hejlsberg",
            },
            new User(TenantUserId, "name2", "qwe2@banana.com",tenantId:TenantId),
        };
        users[0].AddRole(RoleIdOfAnders);
        users[0].AddClaim(
            Guid.NewGuid(),
            ClaimOfAnders.Type,
            ClaimOfAnders.Value);
        users[0].AddLogin(
            LoginOfAnders.LoginProvider,
            LoginOfAnders.ProviderKey,
            LoginOfAnders.ProviderDisplayName);
        users[0].AddOrUpdateToken(
            LoginOfAnders.LoginProvider,
            "Authentication",
            _fixture.Create<string>());

        foreach (var user in users)
            await _userRepository.AddAsync(user);
    }

    [UnitOfWork]
    protected virtual async Task AddRolesAsync()
    {
        var roles = new[]
        {
            new Role(RoleAuthorId, RoleAuthor),
            new Role(RoleIdOfAnders, RoleNameOfAnders),
            _fixture.Create<Role>()
        };
        foreach (var role in roles)
        {
            role.AddClaim(
                _fixture.Create<Guid>(),
                _fixture.Create<string>(),
                _fixture.Create<string>());
            await _roleRepository.AddAsync(role);
        }
    }
}