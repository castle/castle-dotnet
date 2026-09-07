using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class ListResponse : IHasJson
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public string PrimaryField { get; set; }

        public string SecondaryField { get; set; }

        public string Description { get; set; }

        public long? DefaultItemArchivationTime { get; set; }

        public DateTime? ArchivedAt { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Captures any fields returned by the API that are not mapped to a typed
        /// property, so new fields are accessible without an SDK release. Keys are the
        /// raw API names (snake_case).
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; } = new Dictionary<string, JToken>();

        public JObject Internal { get; set; }
    }
}
