namespace Castle.Messages.Responses
{
    public class BatchListItemsResponse
    {
        public long TotalReceived { get; set; }

        public long TotalProcessed { get; set; }

        public long Created { get; set; }

        public long Updated { get; set; }

        public long Replaced { get; set; }

        public long Errored { get; set; }
    }
}
