namespace Castle.Messages.Requests
{
    public class UpdateListRequest
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public string Description { get; set; }

        public long? DefaultItemArchivationTime { get; set; }
    }
}
