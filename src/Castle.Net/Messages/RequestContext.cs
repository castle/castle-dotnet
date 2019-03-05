using System.Collections.Generic;
using Newtonsoft.Json;

namespace Castle.Net.Messages
{
    public class RequestContext
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        public string Ip { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public LibraryInfo Library { get; set; }
    }
}