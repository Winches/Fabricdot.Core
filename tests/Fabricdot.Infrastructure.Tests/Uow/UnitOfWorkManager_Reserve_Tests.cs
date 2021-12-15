using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.Test.Shared;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UnitOfWorkManager_Reserve_Tests : IntegrationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkManager_Reserve_Tests()
        {
            var provider = ServiceScope.ServiceProvider;
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
        }

        [Fact]
        public async Task Reserve_NoExistedUow_Correctly()
        {
            const string reservationName = "1";
            using (var reservedUow = _unitOfWorkManager.Reserve(reservationName))
            {
                AssertReservedUow(reservationName, reservedUow);

                using (var nestedUow = _unitOfWorkManager.Begin())
                {
                    var available = _unitOfWorkManager.Available;
                    Assert.Same(nestedUow, available);
                    Assert.NotEqual(reservedUow.Id, nestedUow.Id);
                    await nestedUow.CommitChangesAsync();
                }

                await reservedUow.CommitChangesAsync();
            }
        }

        [Fact]
        public async Task BeginReserve_WithReservedUow_Correctly()
        {
            const string reservationName = "1";
            IUnitOfWork available;
            using (var reservedUow = _unitOfWorkManager.Reserve(reservationName))
            {
                AssertReservedUow(reservationName, reservedUow);

                using (var uow = _unitOfWorkManager.Begin())
                {
                    available = _unitOfWorkManager.Available;
                    Assert.Same(uow, available);
                    await uow.CommitChangesAsync();
                }

                _unitOfWorkManager.BeginReserved(reservationName);
                available = _unitOfWorkManager.Available;
                Assert.Same(reservedUow, available);
                Assert.True(available.IsActive);

                await available.CommitChangesAsync();
            }
        }

        [Fact]
        public void BeginReserve_NoReservedUow_ThrowException()
        {
            void testCode() => _unitOfWorkManager.BeginReserved("1");
            Assert.Throws<InvalidOperationException>(testCode);
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.RegisterModules(new InfrastructureModule());
        }

        private void AssertReservedUow(string name, IUnitOfWork unitOfWork)
        {
            Assert.Equal(name, unitOfWork.ReservationName);
            Assert.False(unitOfWork.IsActive);
            var available = _unitOfWorkManager.Available;
            Assert.Null(available);
        }
    }
}