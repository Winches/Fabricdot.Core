using Fabricdot.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.ValueConversion;

public class SingleValueObjectConverter<T, TValue> : ValueConverter<T, TValue> where T : SingleValueObject<TValue> where TValue : IComparable
{
    public SingleValueObjectConverter() : base(v => v.Value, v => (T)Activator.CreateInstance(typeof(T), v)!)
    {
    }
}
