using System.Diagnostics.CodeAnalysis;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UnitOfWork_Dispose_Tests : IntegrationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWork_Dispose_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public void Dispose_InvokeOnce_Inactive()
        {
            var uow = _unitOfWorkManager.Begin();
            uow.Dispose();
            Assert.False(uow.IsActive);
        }

        [Fact]
        public void Dispose_InvokeTwice_DoNothing()
        {
            var uow = _unitOfWorkManager.Begin();
            uow.Dispose();
            uow.Dispose();
            Assert.False(uow.IsActive);
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
        }
    }
}