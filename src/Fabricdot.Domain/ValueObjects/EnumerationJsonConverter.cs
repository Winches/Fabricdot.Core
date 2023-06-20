using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fabricdot.Domain.ValueObjects;

public class EnumerationJsonConverter<T> : JsonConverter<T> where T : Enumeration
{
    private static readonly MethodInfo s_factoryMethodInfo = typeof(Enumeration).GetMethod(
        nameof(Enumeration.FromValue),
        BindingFlags.Static | BindingFlags.Public)!;

    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableTo(typeof(Enumeration));

    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var factory = s_factoryMethodInfo.MakeGenericMethod(typeToConvert);
        return (T?)factory.Invoke(null, new object[] { reader.GetInt32() });
    }

    public override void Write(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}
