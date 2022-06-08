using Fabricdot.Core.Validation;

// ReSharper disable once CheckNamespace
namespace System.Text.Json.Serialization;

public class NotificationErrorConverter : JsonConverter<Notification.Error>
{
    /// <inheritdoc />
    public override Notification.Error Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return new Notification.Error(reader.GetString()!);
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        Notification.Error value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}