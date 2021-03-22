using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class RequestOptions
    {
        [JsonConverter(typeof(EmptyStringToFalseConverter))]
        public string Fingerprint { get; set; }

        public string Ip { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringScrubConverter))]
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        [JsonProperty]
        internal LibraryInfo Library { get; set; } = new LibraryInfo();

        internal RequestOptions WithHeaders(IDictionary<string, string> headers)
        {
            return new RequestOptions()
            {
                Fingerprint = Fingerprint,
                Ip = Ip,
                Library = Library,
                Headers = headers
            };
        }
    }
}