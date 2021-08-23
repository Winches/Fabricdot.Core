using System.Diagnostics.CodeAnalysis;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UnitOfWork_Events_Tests : IntegrationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWork_Events_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public void Dispose_SubscribeDisposed_TriggerEventHandler()
        {
            var disposed = false;
            using (var uow = _unitOfWorkManager.Begin())
            {
                uow.Disposed += (sender, e) => disposed = true;
                Assert.False(disposed);
            }
            Assert.True(disposed);
        }

        [Fact]
        public void Dispose_SubscribeChildUowDisposed_TriggerEventHandler()
        {
            var disposed = false;
            using (var uow1 = _unitOfWorkManager.Begin())
            {
                using (var uow2 = _unitOfWorkManager.Begin())
                {
                    uow1.Disposed += (sender, e) => disposed = true;
                }
                Assert.False(disposed);
            }
            Assert.True(disposed);
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
        }
    }
}