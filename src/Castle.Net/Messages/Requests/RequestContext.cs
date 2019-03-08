using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class RequestContext
    {
        [JsonConverter(typeof(EmptyStringToFalseConverter))]
        public string ClientId { get; set; }

        public string Ip { get; set; }

        [JsonProperty(ItemConverterType = typeof(HeaderConverter))]
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