using System;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IUnitOfWork = Fabricdot.Infrastructure.Core.Uow.Abstractions.IUnitOfWork;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Uow
{
    public class UnitOfWorkDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>, ITransientDependency
        where TDbContext : DbContext
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IConnectionStringResolver _connectionStringResolver;

        public UnitOfWorkDbContextProvider(
            IUnitOfWorkManager unitOfWorkManager,
            IConnectionStringResolver connectionStringResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
        }

        /// <inheritdoc />
        public virtual async Task<TDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            var unitOfWork = _unitOfWorkManager.Available;
            if (unitOfWork == null)
                throw new InvalidOperationException("There is no available unit-of-work");

            var databaseKey = GetFacadeKey(false);
            var database = unitOfWork.Facade.GetDatabase(databaseKey); //find by dbContext type and connection string

            if (database == null)
            {
                var dbContext = await CreateDbContextAsync(unitOfWork, cancellationToken);
                database = new EfDatabaseFacade(dbContext);
                unitOfWork.Facade.AddDatabase(databaseKey, database);
            }

            return ((EfDatabaseFacade)database).DbContext as TDbContext;
        }

        protected virtual string GetFacadeKey(bool isTransaction)
        {
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = _connectionStringResolver.ResolveAsync(connectionStringName).GetAwaiter().GetResult();
            return $"{(isTransaction ? "EfTransaction" : typeof(TDbContext).FullName)}_{connectionString}";
        }

        protected virtual async Task<TDbContext> CreateDbContextAsync(
            IUnitOfWork unitOfWork,
            CancellationToken cancellationToken)
        {
            var unitOfWorkOptions = unitOfWork.Options;
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (unitOfWorkOptions.IsTransactional)
                await CreateTransaction(unitOfWork, dbContext, cancellationToken);
            return dbContext;
        }

        protected virtual async Task CreateTransaction(
            IUnitOfWork unitOfWork,
            DbContext dbContext,
            CancellationToken cancellationToken)
        {
            var unitOfWorkOptions = unitOfWork.Options;
            var key = GetFacadeKey(true);
            var transaction = unitOfWork.Facade.GeTransaction(key) as EfTransactionFacade; //find by connection string
            if (transaction == null)
            {
                var dbContextTransaction =
                    await dbContext.Database.BeginTransactionAsync(unitOfWorkOptions.IsolationLevel, cancellationToken);
                var efTransaction = new EfTransactionFacade(dbContextTransaction, dbContext);
                unitOfWork.Facade.AddTransaction(key, efTransaction);
            }
            else //different dbContext with same connection string
            {
                var dbContextTransaction = transaction.DbContextTransaction;
                if (!await dbContext.TryUseTransactionAsync(dbContextTransaction, cancellationToken))
                {
                    await dbContext.BeginTransactionAsync(unitOfWorkOptions.IsolationLevel, cancellationToken);
                }

                transaction.AttendedDbContexts.Add(dbContext);
            }
        }
    }
}