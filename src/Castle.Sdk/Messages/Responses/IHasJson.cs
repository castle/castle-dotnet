using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public interface IHasJson
    {
        JObject Internal { get; set; }
    }
}
