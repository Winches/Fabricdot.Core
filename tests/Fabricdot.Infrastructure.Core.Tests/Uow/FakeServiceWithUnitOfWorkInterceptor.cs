using System;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    [UnitOfWorkInterceptor]
    public class FakeServiceWithUnitOfWorkInterceptor
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FakeServiceWithUnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual void UseAutomaticTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        [UnitOfWork(true)]
        public virtual void UseTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        [UnitOfWork(false)]
        public virtual void UseNotTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        private void UseUow(Action<IUnitOfWork> action)
        {
            var unitOfWork = _unitOfWorkManager.Available;
            action.Invoke(unitOfWork);
        }
    }
}