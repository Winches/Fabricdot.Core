using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class UnitOfWorkManager_Reserve_Tests : IntegrationTestBase<InfrastructureTestModule>
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public UnitOfWorkManager_Reserve_Tests()
    {
        _unitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    }

    [Fact]
    public async Task Reserve_NoExistedUow_Correctly()
    {
        var reservationName = Create<string>();
        using (var reservedUow = _unitOfWorkManager.Reserve(reservationName))
        {
            AssertReservedUow(reservationName, reservedUow);

            using (var nestedUow = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Available.Should().Be(nestedUow);
                nestedUow.Id.Should().NotBe(reservedUow.Id);

                await nestedUow.CommitChangesAsync();
            }

            await reservedUow.CommitChangesAsync();
        }
    }

    [Fact]
    public async Task BeginReserve_WithReservedUow_Correctly()
    {
        var reservationName = Create<string>();
        using (var reservedUow = _unitOfWorkManager.Reserve(reservationName))
        {
            AssertReservedUow(reservationName, reservedUow);

            using (var uow = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Available.Should().Be(uow);

                await uow.CommitChangesAsync();
            }

            _unitOfWorkManager.BeginReserved(reservationName);
            _unitOfWorkManager.Available.Should().Be(reservedUow);

            await reservedUow.CommitChangesAsync();
        }
    }

    [Fact]
    public void BeginReserve_NoReservedUow_ThrowException()
    {
        Invoking(() => _unitOfWorkManager.BeginReserved(Create<string>()))
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    private void AssertReservedUow(string name, IUnitOfWork unitOfWork)
    {
        unitOfWork.ReservationName.Should().Be(name);
        unitOfWork.IsActive.Should().BeFalse();
        _unitOfWorkManager.Available.Should().BeNull();
    }
}