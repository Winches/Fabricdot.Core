using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Core.Uow.Abstractions
{
    public interface IUnitOfWorkFacade : IDisposable
    {
        IReadOnlyCollection<IDatabaseFacade> Databases { get; }
        IReadOnlyCollection<ITransactionFacade> Transactions { get; }

        void AddDatabase(string key, IDatabaseFacade database);

        IDatabaseFacade GetDatabase(string key);

        void AddTransaction(string key, ITransactionFacade transaction);

        ITransactionFacade GeTransaction(string key);
    }
}