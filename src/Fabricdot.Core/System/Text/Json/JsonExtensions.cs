using System.Text.Json;

namespace Fabricdot.Core.System.Text.Json
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions(JsonSerializerDefaults.General);

        public static T FromJson<T>(
            this string json,
            JsonSerializerOptions options = null)
        {
            return JsonSerializer.Deserialize<T>(json, options ?? _options);
        }

        public static string ToJson<T>(
            this T obj,
            JsonSerializerOptions options = null)
        {
            return JsonSerializer.Serialize(obj, options ?? _options);
        }
    }
}