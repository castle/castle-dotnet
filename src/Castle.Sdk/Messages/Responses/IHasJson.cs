using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    internal interface IHasJson
    {
        JObject Internal { get; set; }
    }
}
