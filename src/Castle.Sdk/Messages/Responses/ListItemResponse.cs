using System;
using Castle.Messages.Requests;
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

        public JObject Internal { get; set; }
    }
}
