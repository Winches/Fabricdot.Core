using System;
using System.Threading.Tasks;
using Fabricdot.Identity.Tests.Data;
using Fabricdot.Identity.Tests.Entities;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Fabricdot.Identity.Tests
{
    public abstract class IdentityTestBase : EntityFrameworkCoreTestsBase
    {
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            serviceCollection.AddIdentity<User, Role>()
                 .AddRepositories<FakeDbContext>()
                 .AddDefaultClaimsPrincipalFactory()
                 .AddDefaultTokenProviders();

            var mockCurrentTenant = new Mock<ICurrentTenant>();
            mockCurrentTenant.Setup(v => v.Id).Returns((Guid?)null);
            serviceCollection.AddSingleton(mockCurrentTenant.Object);
        }

        protected async Task UseUowAsync(Func<Task> func)
        {
            var unitOfWorkManager = ScopeServiceProvider.GetRequiredService<IUnitOfWorkManager>();
            using var uow = unitOfWorkManager.Begin();
            await func();
            await uow.CommitChangesAsync();
        }
    }
}