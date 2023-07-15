using Fabricdot.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.ValueConversion;

public class EnumerationConverter<T> : ValueConverter<T, int> where T : Enumeration
{
    public EnumerationConverter() : base(v => v.Value, v => Enumeration.FromValue<T>(v))
    {
    }
}
