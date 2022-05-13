using Ardalis.GuardClauses;

namespace System.Text.Json
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.General);

        public static T FromJson<T>(
            this string json,
            JsonSerializerOptions options = null)
        {
            Guard.Against.Null(json, nameof(json));

            return JsonSerializer.Deserialize<T>(json, options ?? _options);
        }

        public static T FromJson<T>(
            this string json,
            T typeObject,
            JsonSerializerOptions options = null)
        {
            Guard.Against.Null(typeObject, nameof(typeObject));
            return json.FromJson<T>(options);
        }

        public static string ToJson<T>(
            this T obj,
            JsonSerializerOptions options = null)
        {
            Guard.Against.Null(obj, nameof(obj));

            return JsonSerializer.Serialize(obj, options ?? _options);
        }
    }
}