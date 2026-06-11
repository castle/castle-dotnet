using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class Location
    {
        public string CountryCode { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string RegionCode { get; set; }

        public string City { get; set; }

        public float Lat { get; set; }

        public float Lon { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();
    }
}