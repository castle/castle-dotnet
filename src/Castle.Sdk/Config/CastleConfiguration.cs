﻿using System;
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
            AllowList = Headers.AllowList;
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
        /// AllowList for headers in request context object
        /// </summary>
        public string[] AllowList { get; set; } = { };

        /// <summary>
        /// DenyList for headers in request context object
        /// </summary>
        public string[] DenyList { get; set; } = { };

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
