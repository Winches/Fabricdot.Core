using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Repositories
{
    public abstract class EfRepositoryTestsBase : EntityFrameworkCoreTestsBase
    {
        protected FakeDbContext FakeDbContext { get; }

        public EfRepositoryTestsBase()
        {
            var provider = ServiceScope.ServiceProvider;
            FakeDbContext = GetDbContextAsync<FakeDbContext>().GetAwaiter().GetResult();
        }

        protected async Task<TDbContext> GetDbContextAsync<TDbContext>() where TDbContext : DbContext
        {
            var provider = ServiceScope.ServiceProvider;
            var dbContextProvider = provider.GetRequiredService<IDbContextProvider<TDbContext>>();
            return await dbContextProvider.GetDbContextAsync();
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            var serviceDescriptor = new ServiceDescriptor(
                typeof(IDbContextProvider<>),
                typeof(DefaultDbContextProvider<>),
                ServiceLifetime.Transient);
            serviceCollection.Replace(serviceDescriptor);
        }
    }
}