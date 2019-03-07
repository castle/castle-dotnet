namespace Castle.Messages.Responses
{
    public class DeviceList
    {
        public int TotalCount { get; set; }

        public Device[] Data { get; set; } = { };
    }
}