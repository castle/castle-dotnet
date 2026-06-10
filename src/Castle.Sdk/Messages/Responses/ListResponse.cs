using System;
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

        public JObject Internal { get; set; }
    }
}
