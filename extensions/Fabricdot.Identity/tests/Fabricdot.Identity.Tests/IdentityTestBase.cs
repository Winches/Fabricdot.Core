using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Identity.Tests;

public abstract class IdentityTestBase : IntegrationTestBase<IdentityTestModule>
{
    protected IdentityTestBase()
    {
        var dataBuilder = ServiceProvider.GetRequiredService<FakeDataBuilder>();
        dataBuilder.BuildAsync().GetAwaiter().GetResult();
    }

    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        UseServiceProviderFactory<FabricdotServiceProviderFactory>();
    }

    protected async Task UseUowAsync(Func<Task> func)
    {
        var unitOfWorkManager = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        using var uow = unitOfWorkManager.Begin();
        await func();
        await uow.CommitChangesAsync();
    }
}