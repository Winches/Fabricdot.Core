using System.Text.Json;

namespace Fabricdot.Core.System.Text.Json
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        public static T FromJson<T>(this string json) => JsonSerializer.Deserialize<T>(json, JsonOptions);

        public static string ToJson<T>(this T obj) => JsonSerializer.Serialize(obj, JsonOptions);
    }
}