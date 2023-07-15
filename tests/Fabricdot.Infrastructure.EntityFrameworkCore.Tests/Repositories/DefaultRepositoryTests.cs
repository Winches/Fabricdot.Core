using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories;

public class DefaultRepositoryTests : EntityFrameworkCoreTestsBase
{
    [Fact]
    public void AddDefaultRepository_Should_RegisterService()
    {
        ServiceProvider.GetService<IRepository<DummyEntity, Guid>>().Should().NotBeNull();
    }
}
