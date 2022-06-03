using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Uow
{
    /// <summary>
    ///     Inspire by ABP unit-of-work
    /// </summary>
    [Dependency(ServiceLifetime.Transient)]
    public class UnitOfWork : IUnitOfWork
    {
        protected UnitOfWorkState State;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _pending;

        /// <inheritdoc />
        public Guid Id { get; }

        /// <inheritdoc />
        public IDictionary<object, object> Items { get; }

        /// <inheritdoc />
        public IServiceProvider ServiceProvider { get; }

        /// <inheritdoc />
        public IUnitOfWorkFacade Facade { get; }

        /// <inheritdoc />
        public UnitOfWorkOptions Options { get; private set; }

        /// <inheritdoc />
        public string? ReservationName { get; private set; }

        /// <inheritdoc />
        public virtual bool IsActive => State == UnitOfWorkState.Initialized;

        /// <inheritdoc />
        public event EventHandler? Disposed;

        public UnitOfWork(
            IOptions<UnitOfWorkOptions> options,
            IServiceProvider serviceProvider,
            ILogger<UnitOfWork> logger,
            IUnitOfWorkFacade unitOfWorkFacade)
        {
            _logger = logger;
            Id = Guid.NewGuid();
            ServiceProvider = serviceProvider;
            Options = options.Value.Clone();
            Facade = unitOfWorkFacade;
            State = UnitOfWorkState.Allocated;
            Items = new Dictionary<object, object>();
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
            await Facade.SaveChangesAsync(cancellationToken);
            await Facade.CommitAsync(cancellationToken);
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
                Facade.RollbackAsync(default).GetAwaiter().GetResult();

            Facade.Dispose();
            Disposed?.Invoke(this, EventArgs.Empty);

            _logger.LogTrace($"{this} :Disposed");
        }

        /// <inheritdoc />
        public override string ToString() => $"UnitOfWork Id:{Id}";
    }
}