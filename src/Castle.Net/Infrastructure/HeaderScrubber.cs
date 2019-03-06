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
            var scrubbed = new Dictionary<string, string>();

            // Do blacklist first, so we don't stray outside the whitelist
            if (blacklist != null)
            {
                foreach (var key in headers.Keys.Except(blacklist, StringComparer.OrdinalIgnoreCase))
                {
                    scrubbed.Add(key, headers[key]);
                }
            }

            if (whitelist != null && whitelist.Length > 0)
            {
                foreach (var key in scrubbed.Keys.Except(whitelist, StringComparer.OrdinalIgnoreCase).ToList())
                {
                    scrubbed.Remove(key);
                }
            }

            return scrubbed;
        }
    }
}
