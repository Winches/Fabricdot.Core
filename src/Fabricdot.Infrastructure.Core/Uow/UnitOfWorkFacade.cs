using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Uow
{
    public class UnitOfWorkFacade : IUnitOfWorkFacade
    {
        private readonly Dictionary<string, IDatabaseFacade> _databases;
        private readonly Dictionary<string, ITransactionFacade> _transactions;
        public IReadOnlyCollection<IDatabaseFacade> Databases => _databases.Values;
        public IReadOnlyCollection<ITransactionFacade> Transactions => _transactions.Values;

        public UnitOfWorkFacade()
        {
            _databases = new Dictionary<string, IDatabaseFacade>();
            _transactions = new Dictionary<string, ITransactionFacade>();
        }

        public void AddDatabase(string key, IDatabaseFacade database)
        {
            Guard.Against.Null(database, nameof(database));

            if (_databases.ContainsKey(key))
                throw new InvalidOperationException("The database is already registered.");
            _databases.Add(key, database);
        }

        public IDatabaseFacade GetDatabase(string key)
        {
            Guard.Against.NullOrEmpty(key, nameof(key));
            return _databases.GetValueOrDefault(key);
        }

        public void AddTransaction(string key, ITransactionFacade transaction)
        {
            Guard.Against.Null(transaction, nameof(transaction));

            if (_transactions.ContainsKey(key))
                throw new InvalidOperationException("The transaction is already registered.");
            _transactions.Add(key, transaction);
        }

        public ITransactionFacade GeTransaction(string key)
        {
            Guard.Against.NullOrEmpty(key, nameof(key));
            return _transactions.GetValueOrDefault(key);
        }

        public void Dispose()
        {
            foreach (var transaction in Transactions)
            {
                try
                {
                    transaction.Dispose();
                }
                catch
                {
                    //todo:print message
                }
            }
        }
    }
}