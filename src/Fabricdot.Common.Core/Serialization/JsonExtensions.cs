using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fabricdot.Common.Core.Serialization
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings JsonOptions = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static T FromJson<T>(this string json, JsonSerializerSettings serializerSettings = null)
        {
            return JsonConvert.DeserializeObject<T>(json, serializerSettings ?? JsonOptions);
        }

        public static T AnonymousObjectFromJson<T>(
            this string json,
            T type,
            JsonSerializerSettings serializerSettings = null)
        {
            return JsonConvert.DeserializeAnonymousType(json, type, serializerSettings ?? JsonOptions);
        }

        public static string ToJson<T>(this T obj, JsonSerializerSettings serializerSettings = null)
        {
            return JsonConvert.SerializeObject(obj, serializerSettings ?? JsonOptions);
        }
    }
}