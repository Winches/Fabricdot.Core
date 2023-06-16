using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class Money : SingleValueObject<decimal>
{
    public static readonly Money Zero = new(0);

    public Money(decimal value) : base(value)
    {
    }

    public static implicit operator decimal(Money money) => money.Value;

    public static implicit operator Money(decimal value) => new(value);
}
