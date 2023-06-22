using Ardalis.GuardClauses;
using Fabricdot.Domain.ValueObjects;

namespace Mall.Domain.Aggregates.OrderAggregate;

public class Address : ValueObject
{
    public string Country { get; private set; } = null!;

    public string State { get; private set; } = null!;

    public string City { get; private set; } = null!;

    public string Street { get; private set; } = null!;

    public Address(
        string country,
        string state,
        string city,
        string street)
    {
        Country = Guard.Against.NullOrEmpty(country, nameof(country));
        State = Guard.Against.NullOrEmpty(state, nameof(state));
        City = Guard.Against.NullOrEmpty(city, nameof(city));
        Street = Guard.Against.NullOrEmpty(street, nameof(street));
    }

    private Address()
    {
    }
}
