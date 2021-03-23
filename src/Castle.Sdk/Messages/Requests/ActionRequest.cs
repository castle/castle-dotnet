using System;
using System.Collections.Generic;
using Castle.Infrastructure;
using Castle.Infrastructure.Json;

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

        public string Status { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        [JsonConverter(typeof(EmptyStringToFalseConverter))]
        public string Fingerprint { get; set; }

        public string Ip { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringScrubConverter))]
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> UserTraits { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public RequestContext Context { get; set; } = new RequestContext();

        private RequestOptions Options { get; set; } = new RequestOptions();

        internal ActionRequest PrepareApiCopy(string[] allowList, string[] denyList)
        {
            var copy = (ActionRequest) MemberwiseClone();
            var scrubbed = HeaderScrubber.Scrub(Options.Headers, allowList, denyList);
            var opts = Options.WithHeaders(scrubbed);

            // Assign Fingerprint, IP and Headers from options
            // Newtonsoft.Json doesn't apply custom converter to null values, so this must be empty instead
            var newFingerprint = opts.Fingerprint ?? "";
            copy.Fingerprint = copy.Fingerprint ?? newFingerprint;
            copy.Ip = opts.Ip;
            copy.Headers = opts.Headers;

            copy.Context = Context.WithLibrary();

            copy.SentAt = DateTime.Now;

            return copy;
        }
    }
}
