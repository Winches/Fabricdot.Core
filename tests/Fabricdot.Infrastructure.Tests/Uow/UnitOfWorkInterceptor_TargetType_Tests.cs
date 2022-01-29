using AspectCore.Extensions.DependencyInjection;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    public class UnitOfWorkInterceptor_TargetType_Tests : IntegrationTestBase
    {
        private readonly IFakeServiceWithUowScope _testService;

        public UnitOfWorkInterceptor_TargetType_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _testService = provider.GetRequiredService<IFakeServiceWithUowScope>();
        }

        [Fact]
        public void UnitOfWorkInterceptor_BeginUowAutomatically()
        {
            _testService.UseTransactionalUow(uow =>
            {
                Assert.NotNull(uow);
                Assert.True(uow.IsActive);
            });
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
            serviceCollection.AddTransient<IFakeServiceWithUowScope, FakeServiceWithUowScope>();
            serviceCollection.AddInterceptors();
            UseServiceProviderFactory<DynamicProxyServiceProviderFactory>();
        }
    }
}