using System;
using Fabricdot.Domain.Services;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Tests.Uow
{
    public interface IFakeServiceWithUowScope : IHasUnitOfWorkScope
    {
        [UnitOfWork(true)]
        void UseTransactionalUow(Action<IUnitOfWork> action);
    }
}