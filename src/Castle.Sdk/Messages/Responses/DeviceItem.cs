using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class DeviceItem
    {
        public string Token { get; set; }

        public string Fingerprint { get; set; }

        public float Risk { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastSeenAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? EscalatedAt { get; set; }

        public DateTime? MitigatedAt { get; set; }

        public DeviceContext Context { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();
    }
}