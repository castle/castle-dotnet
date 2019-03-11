using Castle.Messages;

namespace Castle.Config
{
    public class CastleConfiguration
    {
        /// <summary>
        /// Secret used to authenticate with the Castle Api (Required)
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// The response action to return in case of a failover in an Authenticate request
        /// </summary>
        public ActionType FailOverStrategy { get; set; } = ActionType.Allow;

        /// <summary>
        /// Timeout for requests, in milliseconds
        /// </summary>
        public int Timeout { get; set; } = 1000;

        /// <summary>
        /// Base Castle Api url
        /// </summary>
        public string BaseUrl { get; set; } = "https://api.castle.io";

        public bool LogHttp { get; set; } = false;

        /// <summary>
        /// Log level applied by the injected <see cref="ILogger"/> implementation
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        /// <summary>
        /// Whitelist for headers in request context object
        /// </summary>
        public string[] Whitelist { get; set; } = { };

        /// <summary>
        /// Blacklist for headers in request context object
        /// </summary>
        public string[] Blacklist { get; set; } = {"Cookie"};

        /// <summary>
        /// If true, no requests are actually sent to the Caste Api, and Authenticate returns a failover response
        /// </summary>
        public bool DoNotTrack { get; set; } = false;
    }
}
