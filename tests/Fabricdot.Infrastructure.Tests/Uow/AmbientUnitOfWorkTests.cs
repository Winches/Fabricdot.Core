using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Tests.Uow;

public class AmbientUnitOfWorkTests : TestFor<AmbientUnitOfWork>
{
    [Fact]
    public void UnitOfWork_SetNothing_ReturnNull()
    {
        Sut.UnitOfWork.Should().BeNull();
    }

    [AutoMockData]
    [Theory]
    public void UnitOfWork_SetUnitOfWork_ReturnLatest(
        IUnitOfWork uow1,
        IUnitOfWork uow2)
    {
        Sut.UnitOfWork = uow1;
        Sut.UnitOfWork.Should().BeSameAs(uow1);

        Sut.UnitOfWork = uow2;
        Sut.UnitOfWork.Should().BeSameAs(uow2);
    }

    [Fact]
    public void UnitOfWork_SetNull_ThrowException()
    {
        Invoking(() => Sut.UnitOfWork = null)
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [AutoMockData]
    [Theory]
    public void UnitOfWork_DropCurrent_ReturnOuterUow(
        IUnitOfWork uow1,
        IUnitOfWork uow2)
    {
        Sut.UnitOfWork = uow1;
        Sut.UnitOfWork = uow2;

        Sut.DropCurrent();
        Sut.UnitOfWork.Should().Be(uow1);

        Sut.DropCurrent();
        Sut.UnitOfWork.Should().BeNull();
    }

    [AutoMockData]
    [Theory]
    public void GetOuter_GivenUnitOfWork_ReturnOuterUow(
        IUnitOfWork uow1,
        IUnitOfWork uow2)
    {
        Sut.UnitOfWork = uow1;
        Sut.UnitOfWork = uow2;

        Sut.GetOuter(uow1).Should().BeNull();
        Sut.GetOuter(uow2).Should().Be(uow1);
    }

    [Fact]
    public void DropCurrent_NoExistedUow_ThrowException()
    {
        Sut.UnitOfWork.Should().BeNull();
        Invoking(() => Sut.DropCurrent())
                     .Should()
                     .Throw<InvalidOperationException>();
    }

    protected override AmbientUnitOfWork CreateSut() => new();
}
