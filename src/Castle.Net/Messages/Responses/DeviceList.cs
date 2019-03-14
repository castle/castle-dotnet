using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class DeviceList : IHasJson
    {
        public int TotalCount { get; set; }

        public DeviceItem[] Data { get; set; } = { };

        public JObject Internal { get; set; }
    }
}