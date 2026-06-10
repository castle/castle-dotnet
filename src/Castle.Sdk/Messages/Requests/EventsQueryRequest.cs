using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    public class EventsQueryRequest
    {
        public IList<QueryFilter> Filters { get; set; }

        public string QueryType { get; set; }

        public IList<string> Columns { get; set; }

        public int? Page { get; set; }

        public int? ResultsSize { get; set; }
    }
}
