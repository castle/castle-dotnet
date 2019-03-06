using System;
using System.Collections.Generic;

namespace Castle.Messages
{
    public class ActionRequest
    {
        public DateTime? SentAt { get; set; }
 
        public DateTime? Timestamp { get; set; }

        public string DeviceToken { get; set; }

        public string Event { get; set; }

        public string UserId { get; set; }

        public IDictionary<string, string> UserTraits { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public RequestContext Context { get; set; }

        internal ActionRequest ShallowCopy()
        {
            return (ActionRequest) MemberwiseClone();
        }
    }
}
