using Fabricdot.Domain.SharedKernel;
using Fabricdot.Domain.ValueObjects;

namespace Fabricdot.Test.Helpers.Domain.Aggregates.OrderAggregate;

public class Address : ValueObject
{
    public string State { get; }

    public string City { get; }

    public string Street { get; }

    [IgnoreMember]
    public string Remark { get; }

    public Address(
        string state,
        string city,
        string street,
        string remark = null)
    {
        State = state;
        City = city;
        Street = street;
        Remark = remark;
    }

    private Address()
    {
    }
}
