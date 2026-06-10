using System.Collections.Generic;

namespace Castle.Messages.Requests
{
    public class BatchListItemsRequest
    {
        public IList<CreateListItemRequest> Items { get; set; } = new List<CreateListItemRequest>();
    }
}
