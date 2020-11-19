using System;

namespace Fabricdot.Domain.Core.Entities
{
    public abstract class AggregateRootBase<TKey> : EntityBase<TKey>, IAggregateRoot, IHasConcurrencyStamp
    {
        /// <inheritdoc />
        public string ConcurrencyStamp { get; set; }

        protected AggregateRootBase()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
    }
}