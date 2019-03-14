using Castle.Messages.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Castle.Infrastructure.Json
{
    internal static class JsonForCastle
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        public static readonly JsonSerializer Serializer = JsonSerializer.Create(JsonSettings);

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }

        public static T DeserializeObject<T>(string value)
            where T : class, new()
        {
            var obj = JsonConvert.DeserializeObject<T>(value, JsonSettings) ?? new T();
            if (obj is IHasJson withJson)
            {
                withJson.Internal = JObject.Parse(value);
            }

            return obj;
        }

        public static JObject FromObject(object obj)
        {
            return JObject.FromObject(obj, Serializer);
        }
    }
}
