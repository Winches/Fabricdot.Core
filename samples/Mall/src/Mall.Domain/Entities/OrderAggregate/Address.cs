using System.Collections.Generic;
using Ardalis.GuardClauses;
using Fabricdot.Domain.Core.ValueObjects;

namespace Mall.Domain.Entities.OrderAggregate
{
    public class Address : ValueObject
    {
        public string Country { get; private set; }

        public string State { get; private set; }

        public string City { get; private set; }

        public string Street { get; private set; }

        public Address()
        {
        }

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

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Country;
            yield return State;
            yield return City;
            yield return Street;
        }
    }
}