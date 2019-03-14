using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class Device : DeviceItem, IHasJson
    {
        public JObject Internal { get; set; }
    }
}