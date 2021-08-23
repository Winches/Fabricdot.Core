using System;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    public class UnitOfWorkInterceptorTestService : IUnitOfWorkInterceptorTestService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkInterceptorTestService(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void UseAutomaticTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        [UnitOfWork(true)]
        public void UseTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        [UnitOfWork(false)]
        public void UseNotTransactionalUow(Action<IUnitOfWork> action) => UseUow(action);

        private void UseUow(Action<IUnitOfWork> action)
        {
            var unitOfWork = _unitOfWorkManager.Available;
            action.Invoke(unitOfWork);
        }
    }
}