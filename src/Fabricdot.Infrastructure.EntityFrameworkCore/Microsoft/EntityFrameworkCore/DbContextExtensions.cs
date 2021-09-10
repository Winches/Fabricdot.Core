// ReSharper disable CheckNamespace

using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static bool IsRelationalDatabase(this DbContext dbContext)
        {
            Guard.Against.Null(dbContext, nameof(dbContext));
            return dbContext.Database.GetService<IDatabaseCreator>() is IRelationalDatabaseCreator;
        }

        /// <summary>
        ///     Create new transaction
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="isolationLevel"> Non-relational database will ignore it.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IDbContextTransaction> BeginTransactionAsync(
            this DbContext dbContext,
            IsolationLevel? isolationLevel = null,
            CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(dbContext, nameof(dbContext));

            isolationLevel = dbContext.IsRelationalDatabase() ? isolationLevel : null;

            return isolationLevel.HasValue
                ? await dbContext.Database.BeginTransactionAsync(isolationLevel.Value, cancellationToken)
                : await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        /// <summary>
        ///     Try to use existing transaction of relational database
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="dbContextTransaction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>success</returns>
        public static async Task<bool> TryUseTransactionAsync(
            this DbContext dbContext,
            IDbContextTransaction dbContextTransaction,
            CancellationToken cancellationToken = default)
        {
            Guard.Against.Null(dbContext, nameof(dbContext));
            Guard.Against.Null(dbContextTransaction, nameof(dbContextTransaction));

            if (!dbContext.IsRelationalDatabase())
                return false;

            var dbContextConnection = dbContext.Database.GetDbConnection();
            var dbTransaction = dbContextTransaction.GetDbTransaction();
            var transactionConnection = dbTransaction.Connection;

            if (dbContextConnection != transactionConnection)
                return false;

            //share transaction
            await dbContext.Database.UseTransactionAsync(
                dbTransaction, cancellationToken);
            return true;
        }
    }
}