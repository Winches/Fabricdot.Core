using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Core.Uow
{
    public class AmbientUnitOfWork : IAmbientUnitOfWork, ISingletonDependency
    {
        private readonly AsyncLocal<LinkedList<IUnitOfWork>> _uowChain = new AsyncLocal<LinkedList<IUnitOfWork>>();

        /// <inheritdoc />
        public IUnitOfWork UnitOfWork
        {
            get => UowChain?.Last?.Value;
            set
            {
                Guard.Against.Null(value, nameof(value));
                UowChain ??= new LinkedList<IUnitOfWork>();
                UowChain.AddLast(value);
            }
        }

        protected LinkedList<IUnitOfWork> UowChain
        {
            get => _uowChain.Value;
            set => _uowChain.Value = value;
        }

        /// <inheritdoc />
        public void DropCurrent()
        {
            if (UowChain?.Any() != true)
                throw new InvalidOperationException("There is no existed unit of work.");
            UowChain.RemoveLast();
        }

        /// <inheritdoc />
        public IUnitOfWork GetOuter(IUnitOfWork unitOfWork)
        {
            Guard.Against.Null(unitOfWork, nameof(unitOfWork));
            var previous = UowChain?.Find(unitOfWork)?.Previous;
            return previous?.Value;
        }
    }
}