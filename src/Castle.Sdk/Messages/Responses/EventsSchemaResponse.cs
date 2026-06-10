using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class EventsSchemaResponse : IHasJson
    {
        public IList<JObject> Fields { get; set; }

        public IList<JObject> Buckets { get; set; }

        public JObject Internal { get; set; }
    }
}
