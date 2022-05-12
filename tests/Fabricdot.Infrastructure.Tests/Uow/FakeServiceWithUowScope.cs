using System;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    public class FakeServiceWithUowScope : IFakeServiceWithUowScope, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FakeServiceWithUowScope(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork(true)]
        public virtual void UseTransactionalUow(Action<IUnitOfWork> action)
        {
            var unitOfWork = _unitOfWorkManager.Available;
            action.Invoke(unitOfWork);
        }
    }
}