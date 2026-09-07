using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class DeviceContext
    {
        public string Ip { get; set; }

        public Location Location { get; set; }

        public UserAgent UserAgent { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();
    }
}