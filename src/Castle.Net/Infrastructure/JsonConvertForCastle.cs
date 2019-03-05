using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Castle.Net.Infrastructure
{
    internal static class JsonConvertForCastle
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }
    }
}
