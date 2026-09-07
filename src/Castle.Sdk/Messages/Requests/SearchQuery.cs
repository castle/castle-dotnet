using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    /// <summary>
    /// Query payload shared by the list and list-item search endpoints.
    /// </summary>
    public class SearchQuery
    {
        public QuerySort Sort { get; set; }

        public IList<QueryFilter> Filters { get; set; }

        public int? Page { get; set; }

        public int? ResultsSize { get; set; }
    }
}
