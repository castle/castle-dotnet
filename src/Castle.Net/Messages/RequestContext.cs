using System.Collections.Generic;

namespace Castle.Messages
{
    public class RequestContext
    {
        public string ClientId { get; set; }

        public string Ip { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public LibraryInfo Library { get; set; }

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