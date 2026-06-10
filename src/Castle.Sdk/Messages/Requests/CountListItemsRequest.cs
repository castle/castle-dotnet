using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    public class CountListItemsRequest
    {
        public IList<QueryFilter> Filters { get; set; }
    }
}
