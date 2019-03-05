using System;

namespace Castle.Net.Config
{
    public class CastleOptions
    {
        public string ApiSecret { get; set; }

        public FailOverStrategy FailOverStrategy { get; set; } = FailOverStrategy.Allow;

        public int Timeout { get; set; } = 500;

        public Uri BaseUrl { get; set; } = new Uri("https://api.castle.io");

        public bool LogHttp { get; set; } = false;

        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        public string[] Whitelist { get; set; } = { };

        public string[] Blacklist { get; set; } = {"Cookie"};

        public bool DoNotTrack { get; set; } = false;
    }
}
