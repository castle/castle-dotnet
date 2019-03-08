using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Castle.Infrastructure.Json
{
    internal static class JsonConvertForCastle
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

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }

        public static T DeserializeObject<T>(string value)
            where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(value, JsonSettings) ?? new T();
        }
    }
}
