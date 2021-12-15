using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Uow
{
    public class EfTransactionFacade : ITransactionFacade
    {
        public IDbContextTransaction DbContextTransaction { get; }
        public DbContext StarterDbContext { get; }
        public List<DbContext> AttendedDbContexts { get; }

        public EfTransactionFacade(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;
            AttendedDbContexts = new List<DbContext>();
        }

        /// <inheritdoc />
        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            foreach (var dbContext in IgnoreSharedTransaction())
                await dbContext.Database.CommitTransactionAsync(cancellationToken);
            await DbContextTransaction.CommitAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            foreach (var dbContext in IgnoreSharedTransaction())
                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
            await DbContextTransaction.RollbackAsync(cancellationToken);
        }

        /// <inheritdoc />
        public virtual void Dispose() => DbContextTransaction.Dispose();

        protected virtual IEnumerable<DbContext> IgnoreSharedTransaction()
        {
            //Relational databases use the shared transaction if they are using the same connection
            var connection = DbContextTransaction.GetDbTransaction().Connection;
            return AttendedDbContexts.Where(v =>
                !(v.IsRelationalDatabase() && v.Database.GetDbConnection() == connection));
        }
    }
}