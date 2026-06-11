using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class UserAgent
    {
        public string Raw { get; set; }

        public string Browser { get; set; }

        public string Version { get; set; }

        public string Os { get; set; }

        public bool Mobile { get; set; }

        public string Platform { get; set; }

        public string Device { get; set; }

        public string Family { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();
    }
}