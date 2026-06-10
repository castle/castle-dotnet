using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class EventsResponse : IHasJson
    {
        public IList<JObject> Data { get; set; }

        public long TotalCount { get; set; }

        public JObject Internal { get; set; }
    }
}
