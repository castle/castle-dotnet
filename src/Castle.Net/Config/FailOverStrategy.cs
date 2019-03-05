using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Castle.Net.Config
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FailOverStrategy
    {
        Allow = 0,
        Challenge,
        Deny,
        None
    }
}