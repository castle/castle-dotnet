namespace Castle.Messages.Requests
{
    public class CreateListRequest
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public string PrimaryField { get; set; }

        public string SecondaryField { get; set; }

        public string Description { get; set; }

        public long? DefaultItemArchivationTime { get; set; }
    }
}
