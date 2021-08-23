using System;
using Fabricdot.Infrastructure.Core.Uow;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Tests.Uow
{
    [UnitOfWork]
    public interface IUnitOfWorkInterceptorTestService
    {
        void UseTransactionalUow(Action<IUnitOfWork> action);

        void UseNotTransactionalUow(Action<IUnitOfWork> action);

        void UseAutomaticTransactionalUow(Action<IUnitOfWork> action);
    }
}