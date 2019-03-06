using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Castle.Messages
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ActionType
    {
        None = 0,
        Allow,
        Challenge,
        Deny
    }
}