using System;
using System.Collections.Generic;
using Castle.Messages.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Castle.Messages.Responses
{
    public class ListItemResponse : IHasJson
    {
        public string Id { get; set; }

        public string ListId { get; set; }

        public string PrimaryValue { get; set; }

        public string SecondaryValue { get; set; }

        public string Comment { get; set; }

        public ListItemAuthor Author { get; set; }

        public DateTime? AutoArchivesAt { get; set; }

        public bool Archived { get; set; }

        public DateTime? CreatedAt { get; set; }

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
