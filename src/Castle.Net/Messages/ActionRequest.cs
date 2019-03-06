using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Castle.Net.Messages
{
    public abstract class ActionRequest
    {
        [JsonProperty("sent_at")]
        public DateTime? SentAt { get; set; }
 
        public DateTime? Timestamp { get; set; }

        [JsonProperty("device_token")]
        public string DeviceToken { get; set; }

        public string Event { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_traits")]
        public IDictionary<string, string> UserTraits { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public RequestContext Context { get; set; }
    }
}
