using System.Collections.Immutable;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Infrastructure.Uow;

[Dependency(ServiceLifetime.Transient)]
public class UnitOfWorkFacade : IUnitOfWorkFacade, ISupportSaveChanges
{
    private readonly Dictionary<string, IDatabaseFacade> _databases;
    private readonly Dictionary<string, ITransactionFacade> _transactions;
    private readonly ILogger<UnitOfWorkFacade> _logger;

    public IReadOnlyCollection<IDatabaseFacade> Databases => _databases.Values;
    public IReadOnlyCollection<ITransactionFacade> Transactions => _transactions.Values;

    public UnitOfWorkFacade(ILogger<UnitOfWorkFacade> logger)
    {
        _databases = new Dictionary<string, IDatabaseFacade>();
        _transactions = new Dictionary<string, ITransactionFacade>();
        _logger = logger;
    }

    public void AddDatabase(string key, IDatabaseFacade database)
    {
        Guard.Against.Null(database, nameof(database));

        if (_databases.ContainsKey(key))
            throw new InvalidOperationException("The database is already registered.");
        _databases.Add(key, database);
    }

    public IDatabaseFacade? GetDatabase(string key)
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

    public ITransactionFacade? GeTransaction(string key)
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
            catch (Exception e)
            {
                _logger.LogError(e, "Disposing transaction failed");
            }
        }
    }

    public virtual async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var visited = await SaveAsync();
        // TODO: Consider handle domain events inside uow.
        // Domain event handler may add new database.
        while (Databases.Except(visited).Any())
        {
            visited = await SaveAsync();
        }

        async Task<ICollection<IDatabaseFacade>> SaveAsync()
        {
            var databases = Databases.ToList();
            foreach (var database in databases)
            {
                if (database is ISupportSaveChanges supportSaveChanges)
                {
                    await supportSaveChanges.SaveChangesAsync(cancellationToken);
                }
            }
            return databases;
        }
    }

    public virtual async Task CommitAsync(CancellationToken cancellationToken)
    {
        foreach (var transaction in Transactions)
            await transaction.CommitAsync(cancellationToken);
    }

    public virtual async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var transaction in Transactions)
            try
            {
                await transaction.RollbackAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Rollback transaction failed");
            }
    }
}