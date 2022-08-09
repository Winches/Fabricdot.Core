using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fabricdot.Domain.ValueObjects;

public class SingleValueObjectJsonConverter<T> : JsonConverter<T> where T : ISingleValueObject
{
    private static readonly ConcurrentDictionary<Type, Type> _constructorArgumentTypes = new();

    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsAssignableToGenericType(typeof(SingleValueObject<>));

    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var valueType = _constructorArgumentTypes.GetOrAdd(
            typeToConvert,
            _ =>
            {
                var ctor = typeToConvert.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
                var parameterInfo = ctor.GetParameters().Single();
                return parameterInfo.ParameterType;
            });

        var value = JsonSerializer.Deserialize(ref reader, valueType, options);
        return (T?)Activator.CreateInstance(typeToConvert, value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        T value,
        JsonSerializerOptions options)
    {
        var val = value.GetValue();
        JsonSerializer.Serialize(writer, val, val.GetType(), options);
    }
}