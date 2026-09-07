using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    public class EventsGroupRequest
    {
        public IList<QueryFilter> Filters { get; set; }

        public EventsGroupBy GroupBy { get; set; }

        public IList<string> Columns { get; set; }

        public string QueryType { get; set; }

        public QuerySort Sort { get; set; }

        public int? Page { get; set; }

        public int? ResultsSize { get; set; }
    }
}
