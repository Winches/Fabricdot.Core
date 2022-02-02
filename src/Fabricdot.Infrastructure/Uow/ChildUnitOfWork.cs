using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow
{
    public class ChildUnitOfWork : IUnitOfWork
    {
        private readonly IUnitOfWork _parent;

        /// <inheritdoc />
        public Guid Id => _parent.Id;

        /// <inheritdoc />
        public bool IsActive => _parent.IsActive;

        /// <inheritdoc />
        public IUnitOfWorkFacade Facade => _parent.Facade;

        /// <inheritdoc />
        public UnitOfWorkOptions Options => _parent.Options;

        /// <inheritdoc />
        public string ReservationName => _parent.ReservationName;

        /// <inheritdoc />
        public IDictionary<object, object> Items => _parent.Items;

        /// <inheritdoc />
        public IServiceProvider ServiceProvider => _parent.ServiceProvider;

        /// <inheritdoc />
        public event EventHandler Disposed
        {
            add => _parent.Disposed += value;
            remove => _parent.Disposed -= value;
        }

        public ChildUnitOfWork(IUnitOfWork unitOfWork)
        {
            _parent = unitOfWork;
        }

        /// <inheritdoc />
        public virtual void Reserve(string name) => _parent.Reserve(name);

        /// <inheritdoc />
        public void Initialize(UnitOfWorkOptions options) => _parent.Initialize(options);

        /// <inheritdoc />
        public Task CommitChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        /// <inheritdoc />
        public override string ToString() => _parent.ToString();

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}