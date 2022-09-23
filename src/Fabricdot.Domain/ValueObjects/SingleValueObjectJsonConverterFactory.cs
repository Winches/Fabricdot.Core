using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fabricdot.Domain.ValueObjects;

public class SingleValueObjectJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(ISingleValueObject));

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return (JsonConverter?)Activator.CreateInstance(typeof(SingleValueObjectJsonConverter<>).MakeGenericType(typeToConvert));
    }
}