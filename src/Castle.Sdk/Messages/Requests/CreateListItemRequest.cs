using System;

namespace Castle.Messages.Requests
{
    public class CreateListItemRequest
    {
        public string PrimaryValue { get; set; }

        public string SecondaryValue { get; set; }

        public ListItemAuthor Author { get; set; }

        public string Comment { get; set; }

        public DateTime? AutoArchivesAt { get; set; }

        public string Mode { get; set; }
    }
}
