using System;
using System.Collections.Generic;
using System.Linq;

namespace Castle.Infrastructure
{
    internal static class HeaderScrubber
    {
        public static IDictionary<string, string> Scrub(
            IDictionary<string, string> headers,
            string[] whitelist,
            string[] blacklist)
        {
            return headers
                .Select(header => Scrub(whitelist, blacklist, header))
                .ToDictionary(x => x.Key, y => y.Value);
        }

        private static KeyValuePair<string, string> Scrub(
            string[] whitelist, 
            string[] blacklist, 
            KeyValuePair<string, string> header)
        {
            // Scrub to "true" so the custom JsonConverter can find it and convert to actual boolean
            const string scrubValue = "true";

            if (blacklist.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
            {
                return new KeyValuePair<string, string>(header.Key, scrubValue);
            }

            if (whitelist.Length > 0 && !whitelist.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
            {
                return new KeyValuePair<string, string>(header.Key, scrubValue);
            }

            return new KeyValuePair<string, string>(header.Key, header.Value);
        }
    }
}
