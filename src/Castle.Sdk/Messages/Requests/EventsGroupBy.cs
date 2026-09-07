using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    public class EventsGroupBy
    {
        public IList<string> Fields { get; set; }

        public IList<QueryFilter> Filters { get; set; }
    }
}
