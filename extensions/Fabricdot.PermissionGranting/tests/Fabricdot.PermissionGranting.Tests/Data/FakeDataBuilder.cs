using Fabricdot.Authorization;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Core.UniqueIdentifier;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.PermissionGranting.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.PermissionGranting.Tests.Data;

[Dependency(ServiceLifetime.Transient)]
public class FakeDataBuilder
{
    public static readonly GrantSubject Subject = new(GrantTypes.User, "0");

    public static readonly string[] GrantedObjects = new[] { "object1", "object2" };

    private readonly IGuidGenerator _guidGenerator;
    private readonly IDbContextProvider<FakeDbContext> _dbContextProvider;

    public FakeDataBuilder(
        IGuidGenerator guidGenerator,
        IDbContextProvider<FakeDbContext> dbContextProvider)
    {
        _guidGenerator = guidGenerator;
        _dbContextProvider = dbContextProvider;
    }

    public async Task BuildAsync()
    {
        await AddGrantedPermissionsAsync();
    }

    [UnitOfWork]
    protected virtual async Task AddGrantedPermissionsAsync()
    {
        var dbContext = await _dbContextProvider.GetDbContextAsync();
        foreach (var obj in GrantedObjects)
        {
            var entity = new GrantedPermission(_guidGenerator.Create(), Subject, obj);
            await dbContext.AddAsync(entity);
        }
    }
}