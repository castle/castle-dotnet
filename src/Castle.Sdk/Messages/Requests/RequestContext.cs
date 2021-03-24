using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class RequestContext
    {
        [JsonProperty]
        internal LibraryInfo Library { get; set; } = new LibraryInfo();

        internal RequestContext WithLibrary()
        {
            return new RequestContext()
            {
                Library = Library
            };
        }
    }
}