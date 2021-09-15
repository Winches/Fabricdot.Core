using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    public class UnitOfWorkInterceptorTests : IntegrationTestBase
    {
        private readonly FakeServiceWithUnitOfWorkInterceptor _testService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUnitOfWorkTransactionBehaviourProvider _transactionBehaviourProvider;

        public UnitOfWorkInterceptorTests()
        {
            var provider = ServiceScope.ServiceProvider;
            _testService = provider.GetRequiredService<FakeServiceWithUnitOfWorkInterceptor>();
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
            _transactionBehaviourProvider = provider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>();
        }

        [Fact]
        public void UnitOfWorkInterceptor_SpecificNothing_BeginUowAutomatically()
        {
            _testService.UseAutomaticTransactionalUow(uow =>
            {
                const string actionName = nameof(FakeServiceWithUnitOfWorkInterceptor.UseAutomaticTransactionalUow);
                var behaciour = _transactionBehaviourProvider.GetBehaviour(actionName);
                var isTransactional = uow.Options.IsTransactional;

                Assert.Equal(behaciour, isTransactional);
                Assert.True(uow.IsActive);
            });
            var unitOfWork = _unitOfWorkManager.Available;
            Assert.Null(unitOfWork);
        }

        [Fact]
        public void UnitOfWorkInterceptor_SpecificTransactional_BeginTransactionalUow()
        {
            _testService.UseTransactionalUow(uow =>
            {
                var isTransactional = uow.Options.IsTransactional;
                Assert.True(isTransactional);
                Assert.True(uow.IsActive);
            });
            var unitOfWork = _unitOfWorkManager.Available;
            Assert.Null(unitOfWork);
        }

        [Fact]
        public void UnitOfWorkInterceptor_SpecificNotTransactional_BeginNoTransactionalUow()
        {
            _testService.UseNotTransactionalUow(uow =>
            {
                var isTransactional = uow.Options.IsTransactional;
                Assert.False(isTransactional);
                Assert.True(uow.IsActive);
            });
            var unitOfWork = _unitOfWorkManager.Available;
            Assert.Null(unitOfWork);
        }

        [Fact]
        public void UnitOfWorkInterceptor_WithReservedUow_BeginReservedUow()
        {
            using var uow = _unitOfWorkManager.Reserve(UnitOfWorkManager.RESERVATION_NAME);
            _testService.UseAutomaticTransactionalUow(uow =>
            {
                Assert.True(uow.IsActive);
            });
            var unitOfWork = _unitOfWorkManager.Available;
            Assert.Equal(uow, unitOfWork);
            Assert.True(uow.IsActive);
        }

        protected sealed override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
            serviceCollection.AddTransient<FakeServiceWithUnitOfWorkInterceptor>();
            ServiceProvider = serviceCollection.AddInterceptors();
            ServiceScope = ServiceProvider.CreateScope();
        }
    }
}