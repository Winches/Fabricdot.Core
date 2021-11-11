using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.ValueObjects;

namespace Mall.Domain.Entities.OrderAggregate
{
    public class Money : SingleValueObject<decimal>
    {
        public static readonly Money Zero = new Money(0);

        public Money(decimal value) : base(value)
        {
            Guard.Against.Negative(value, nameof(value));
        }

        public static implicit operator decimal(Money money) => money.Value;

        public static implicit operator Money(decimal value) => new Money(value);
    }
}