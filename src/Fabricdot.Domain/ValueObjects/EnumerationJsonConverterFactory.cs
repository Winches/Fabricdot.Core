using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fabricdot.Domain.ValueObjects;

public class EnumerationJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(Enumeration));

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter?)Activator.CreateInstance(typeof(EnumerationJsonConverter<>).MakeGenericType(typeToConvert));
    }
}
