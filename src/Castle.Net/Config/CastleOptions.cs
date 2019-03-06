using Castle.Net.Messages;

namespace Castle.Net.Config
{
    public class CastleOptions
    {
        public string ApiSecret { get; set; }

        public ActionType FailOverStrategy { get; set; } = ActionType.Allow;

        public int Timeout { get; set; } = 1000;

        public string BaseUrl { get; set; } = "https://api.castle.io";

        public bool LogHttp { get; set; } = false;

        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        public string[] Whitelist { get; set; } = { };

        public string[] Blacklist { get; set; } = {"Cookie"};

        public bool DoNotTrack { get; set; } = false;
    }
}
