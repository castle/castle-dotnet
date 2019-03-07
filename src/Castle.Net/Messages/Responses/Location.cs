namespace Castle.Messages.Responses
{
    public class Location
    {
        public string CountryCode { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string RegionCode { get; set; }

        public string City { get; set; }

        public float Lat { get; set; }

        public float Lon { get; set; }
    }
}