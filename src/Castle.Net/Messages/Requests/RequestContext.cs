using System.Collections.Generic;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class RequestContext
    {
        public string ClientId { get; set; }

        public string Ip { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        [JsonProperty]
        internal LibraryInfo Library { get; set; } = new LibraryInfo();

        internal RequestContext WithHeaders(IDictionary<string, string> headers)
        {
            return new RequestContext()
            {
                ClientId = ClientId,
                Ip = Ip,
                Library = Library,
                Headers = headers
            };
        }
    }
}