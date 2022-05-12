using System;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    [UnitOfWorkInterceptor]
    public class FakeServiceWithUnitOfWorkInterceptor : ITransientDependency
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