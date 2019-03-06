namespace Castle.Messages
{
    public class DeviceContext
    {
        public string Ip { get; set; }

        public Location Location { get; set; }

        public UserAgent UserAgent { get; set; }

        public string Type { get; set; }
    }
}