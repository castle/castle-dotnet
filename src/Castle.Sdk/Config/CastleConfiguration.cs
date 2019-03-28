using System;
using System.Linq;
using Castle.Infrastructure;
using Castle.Messages;

namespace Castle.Config
{
    public class CastleConfiguration
    {
        /// <exception cref="ArgumentException">Thrown when <paramref name="apiSecret"/> is null or empty</exception>>
        public CastleConfiguration(string apiSecret)
        {
            ArgumentGuard.NotNullOrEmpty(apiSecret, nameof(apiSecret));

            ApiSecret = apiSecret;
        }

        /// <summary>
        /// Secret used to authenticate with the Castle Api (Required)
        /// </summary>
        public string ApiSecret { get; }

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

        /// <summary>
        /// Log level applied by the injected <see cref="ICastleLogger"/> implementation
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Error;

        /// <summary>
        /// Whitelist for headers in request context object
        /// </summary>
        public string[] Whitelist { get; set; } = { };

        private string[] _blacklist = { "Cookie" };
        /// <summary>
        /// Blacklist for headers in request context object
        /// </summary>
        public string[] Blacklist
        {
            get => _blacklist;
            set => _blacklist = new [] { "Cookie" }.Concat(value ?? new string[] { }).ToArray();
        }

        /// <summary>
        /// If true, no requests are actually sent to the Castle Api, and Authenticate returns a failover response
        /// </summary>
        public bool DoNotTrack { get; set; } = false;

        /// <summary>
        /// Your own logger implementation, for internal SDK logging
        /// </summary>
        public ICastleLogger Logger { get; set; }
    }
}
