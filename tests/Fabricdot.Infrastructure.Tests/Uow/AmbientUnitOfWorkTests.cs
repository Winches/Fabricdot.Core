using System;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Moq;
using Xunit;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    public class AmbientUnitOfWorkTests
    {
        private readonly IAmbientUnitOfWork _ambientUnitOfWork = new AmbientUnitOfWork();

        [Fact]
        public void UnitOfWork_SetNothing_ReturnNull()
        {
            var currentUow = _ambientUnitOfWork.UnitOfWork;
            Assert.Null(currentUow);
        }

        [Fact]
        public void UnitOfWork_SetUnitOfWork_ReturnLatest()
        {
            var uow1 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow1;
            var currentUow1 = _ambientUnitOfWork.UnitOfWork;
            Assert.Same(uow1, currentUow1);

            var uow2 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow2;
            var currentUow2 = _ambientUnitOfWork.UnitOfWork;
            Assert.Same(uow2, currentUow2);
        }

        [Fact]
        public void UnitOfWork_SetNull_ThrowException()
        {
            void testCode() => _ambientUnitOfWork.UnitOfWork = null;
            Assert.Throws<ArgumentNullException>(testCode);
        }

        [Fact]
        public void UnitOfWork_DropCurrent_ReturnOuterUow()
        {
            var uow1 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow1;
            var uow2 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow2;

            _ambientUnitOfWork.DropCurrent();
            var currentUow1 = _ambientUnitOfWork.UnitOfWork;
            Assert.Equal(uow1, currentUow1);

            _ambientUnitOfWork.DropCurrent();
            var currentUow2 = _ambientUnitOfWork.UnitOfWork;
            Assert.Null(currentUow2);
        }

        [Fact]
        public void GetOuter_GivenUnitOfWork_ReturnPrevious()
        {
            var uow1 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow1;
            var outer1 = _ambientUnitOfWork.GetOuter(uow1);
            Assert.Null(outer1);

            var uow2 = new Mock<IUnitOfWork>().Object;
            _ambientUnitOfWork.UnitOfWork = uow2;
            var outer2 = _ambientUnitOfWork.GetOuter(uow2);
            Assert.Same(uow1, outer2);
        }

        [Fact]
        public void DropCurrent_NoExistedUow_ThrowException()
        {
            void testCode() => _ambientUnitOfWork.DropCurrent();
            var currentUow = _ambientUnitOfWork.UnitOfWork;

            Assert.Null(currentUow);
            Assert.Throws<InvalidOperationException>(testCode);
        }
    }
}