using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.CustomerAggregate;

public class CustomerId : Identity<string>
{
    public CustomerId(string value) : base(value)
    {
    }
}
