using System;
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

        public DateTime? CreatedAt { get; set; }

        public string DeviceToken { get; set; }

        public string RequestToken { get; set; }

        public string Event { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }

        public IDictionary<string, string> UserTraits { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, object> Properties { get; set; }

        public IDictionary<string, object> User { get; set; } = new Dictionary<string, object>();

        public IDictionary<string, object> Transaction { get; set; }

        public IDictionary<string, object> Changeset { get; set; }

        public IDictionary<string, string> Product { get; set; }

        public IDictionary<string, string> AuthenticationMethod { get; set; }

        [JsonProperty("skip_request_token_validation")]
        public bool SkipRequestTokenValidation { get; set; }

        [JsonProperty("skip_context_validation")]
        public bool SkipContextValidation { get; set; }

        public RequestContext Context { get; set; } = new RequestContext();

        /// <summary>
        /// Arbitrary top-level request params not covered by a typed property. Serialized
        /// inline, so new API params can be sent without an SDK release. Keys must use the
        /// API's raw names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; } = new Dictionary<string, object>();

        internal ActionRequest PrepareApiCopy(string[] allowList, string[] denyList)
        {
            var copy = (ActionRequest)MemberwiseClone();
            var scrubbed = HeaderScrubber.Scrub(Context.Headers, allowList, denyList);
            copy.Context = Context.WithHeaders(scrubbed);

            copy.SentAt = DateTime.Now;

            return copy;
        }
    }
}
