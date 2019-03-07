using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class ActionRequest
    {
        [JsonProperty]
        internal DateTime SentAt { get; set; }
 
        public DateTime? Timestamp { get; set; }

        public string DeviceToken { get; set; }

        public string Event { get; set; }

        public string UserId { get; set; }

        public IDictionary<string, string> UserTraits { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public RequestContext Context { get; set; } = new RequestContext();

        internal ActionRequest ShallowCopy()
        {
            return (ActionRequest) MemberwiseClone();
        }
    }
}
