using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Core.Uow
{
    /// <summary>
    ///     Inspire by ABP unit-of-work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        protected UnitOfWorkState State;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _pending;

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public IUnitOfWorkFacade Facade { get; }

        /// <inheritdoc />
        public UnitOfWorkOptions Options { get; private set; }

        /// <inheritdoc />
        public string ReservationName { get; private set; }

        /// <inheritdoc />
        public virtual bool IsActive => State == UnitOfWorkState.Initialized;

        /// <inheritdoc />
        public event EventHandler Disposed;

        public UnitOfWork(IOptions<UnitOfWorkOptions> options, ILogger<UnitOfWork> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? NullLogger<UnitOfWork>.Instance;
            ServiceProvider = serviceProvider;
            Id = Guid.NewGuid();
            Options = options.Value.Clone();
            Facade = new UnitOfWorkFacade();
            State = UnitOfWorkState.Allocated;
        }

        public virtual void Reserve(string name)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            if (State != UnitOfWorkState.Allocated)
                throw new InvalidOperationException("UnitOfWork is already initialized.");

            ReservationName = name;
        }

        /// <inheritdoc />
        public virtual void Initialize(UnitOfWorkOptions options)
        {
            Guard.Against.Null(options, nameof(options));

            if (State != UnitOfWorkState.Allocated)
                throw new InvalidOperationException("UnitOfWork is already initialized.");

            Options = options.Clone();
            ReservationName = null;
            State = UnitOfWorkState.Initialized;
        }

        /// <inheritdoc />
        public virtual async Task CommitChangesAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"{this} :Committing");

            if (_pending || State == UnitOfWorkState.Performed || State == UnitOfWorkState.Disposed)
                throw new InvalidOperationException("UnitOfWork is already performed.");

            _pending = true;
            await SavingChangesAsync(cancellationToken);
            await CommitAsync(cancellationToken);
            State = UnitOfWorkState.Performed;

            _logger.LogTrace($"{this} :Committed");
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            _logger.LogTrace($"{this} :Disposing");

            if (State == UnitOfWorkState.Disposed)
                return;

            State = UnitOfWorkState.Disposed;
            //rollback changes when UOW is not committed.
            if (State == UnitOfWorkState.Initialized)
                RollbackAsync(default).GetAwaiter().GetResult();

            Facade.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);

            _logger.LogTrace($"{this} :Disposed");
        }

        /// <inheritdoc />
        public override string ToString() => $"UnitOfWork Id:{Id}";

        protected virtual async Task SavingChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var facade in Facade.Databases)
                if (facade is ISupportSaveChanges supportSaveChanges)
                    await supportSaveChanges.SaveChangesAsync(cancellationToken);
        }

        protected virtual async Task CommitAsync(CancellationToken cancellationToken)
        {
            foreach (var facade in Facade.Transactions)
                await facade.CommitAsync(cancellationToken);
        }

        protected virtual async Task RollbackAsync(CancellationToken cancellationToken)
        {
            foreach (var facade in Facade.Transactions)
                try
                {
                    await facade.RollbackAsync(cancellationToken);
                }
                catch
                {
                }
        }
    }
}