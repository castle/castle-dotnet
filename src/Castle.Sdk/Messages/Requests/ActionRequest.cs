﻿using System;
using System.Collections.Generic;
using Castle.Infrastructure;
using Newtonsoft.Json;

namespace Castle.Messages.Requests
{
    public class ActionRequest
    {
        [JsonProperty]
        internal DateTime SentAt { get; set; }

        public DateTime? Timestamp { get; set; }

        public string DeviceToken { get; set; }

        public string RequestToken { get; set; }

        public string Event { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
        public string UserId { get; set; }

        public IDictionary<string, string> UserTraits { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, object> Properties { get; set; }

        public IDictionary<string, object> User { get; set; } = new Dictionary<string, object>();
        public IDictionary<string, string> Product { get; set; }

        public IDictionary<string, string> AuthenticationMethod { get; set; }

        public RequestContext Context { get; set; } = new RequestContext();

        internal ActionRequest PrepareApiCopy(string[] allowList, string[] denyList)
        {
            var copy = (ActionRequest)MemberwiseClone();
            var scrubbed = HeaderScrubber.Scrub(Context.Headers, allowList, denyList);
            copy.Context = Context.WithHeaders(scrubbed);

            copy.SentAt = DateTime.Now;

            // Newtonsoft.Json doesn't apply custom converter to null values, so this must be empty instead
            copy.Context.ClientId = copy.Context.ClientId ?? "";

            return copy;
        }
    }
}
