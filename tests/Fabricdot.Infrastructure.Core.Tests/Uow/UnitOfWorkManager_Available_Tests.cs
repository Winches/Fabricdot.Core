using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UnitOfWorkManager_Available_Tests : IntegrationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkManager_Available_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public void Available_NoExistedUow_ReturnNull()
        {
            var availableUow = _unitOfWorkManager.Available;
            Assert.Null(availableUow);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Available_BeginNestedUow_ReturnCorrectUow(bool requireNew)
        {
            IUnitOfWork availableUow;
            using (var rootUow = _unitOfWorkManager.Begin())
            {
                availableUow = _unitOfWorkManager.Available;
                Assert.Same(rootUow, availableUow);

                using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
                {
                    //ignore child unit of work
                    availableUow = _unitOfWorkManager.Available;
                    if (requireNew)
                        Assert.Same(nestedUow, availableUow);
                    else
                        Assert.Same(rootUow, availableUow);
                    await nestedUow.CommitChangesAsync();
                }

                availableUow = _unitOfWorkManager.Available;
                Assert.Same(rootUow, availableUow);
                await rootUow.CommitChangesAsync();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Available_CommitChanges_ReturnOuterUow(bool requireNew)
        {
            IUnitOfWork availableUow;
            using (var rootUow = _unitOfWorkManager.Begin())
            {
                using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
                {
                    await nestedUow.CommitChangesAsync();
                    availableUow = _unitOfWorkManager.Available;
                    Assert.Same(rootUow, availableUow);
                }

                await rootUow.CommitChangesAsync();
                availableUow = _unitOfWorkManager.Available;
                Assert.Null(availableUow);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Available_DisposeUow_ReturnOuterUow(bool requireNew)
        {
            IUnitOfWork availableUow;
            using (var rootUow = _unitOfWorkManager.Begin())
            {
                using (var nestedUow = _unitOfWorkManager.Begin(requireNew: requireNew))
                {
                    await nestedUow.CommitChangesAsync();
                }
                availableUow = _unitOfWorkManager.Available;
                Assert.Same(rootUow, availableUow);

                await rootUow.CommitChangesAsync();
            }
            availableUow = _unitOfWorkManager.Available;
            Assert.Null(availableUow);
        }

        /// <inheritdoc />
        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
        }
    }
}