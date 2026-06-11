using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Castle.Messages.Responses
{

    public class ScoreItem
    {
        public float Score { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();
    }
    public class RiskResponse : IHasJson
    {
        public DeviceItem Device { get; set; }
        public float Risk { get; set; }

        public Policy Policy { get; set; }
        public JObject Signals { get; set; }

       public Dictionary<string, ScoreItem> Scores { get; set; }

        // FailOver
        public ActionType Action { get; set; }

        public bool Failover { get; set; }

        public string FailoverReason { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();

        /// <summary>
        /// The full raw JSON payload as returned by the API, populated on deserialization.
        /// </summary>
        public JObject Internal { get; set; }
    }

}