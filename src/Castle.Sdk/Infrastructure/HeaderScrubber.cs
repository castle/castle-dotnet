using System;
using System.Collections.Generic;
using System.Linq;

namespace Castle.Infrastructure
{
    internal static class HeaderScrubber
    {
        static readonly string[] _defaultAllowList = new[] { "User-Agent" };
        static readonly string[] _defaultDenyList = new[] { "Cookie", "Authorization" };

        public static IDictionary<string, string> Scrub(
            IDictionary<string, string> headers,
            string[] allowList,
            string[] denyList)
        {
            return headers
                .Select(header => Scrub(allowList, denyList, header))
                .ToDictionary(x => x.Key, y => y.Value);
        }

        private static KeyValuePair<string, string> Scrub(
            string[] allowList,
            string[] denyList,
            KeyValuePair<string, string> header)
        {
            // Scrub to "true" so the custom JsonConverter can find it and convert to actual boolean
            const string scrubValue = "true";
            var headerKey = header.Key.Replace("HTTP_", "");

            if (denyList != null && (denyList.Contains(headerKey, StringComparer.OrdinalIgnoreCase) || _defaultDenyList.Contains(headerKey, StringComparer.OrdinalIgnoreCase)))
            {
                return new KeyValuePair<string, string>(headerKey, scrubValue);
            }

            if (allowList != null && allowList.Length > 0 && (!allowList.Contains(headerKey, StringComparer.OrdinalIgnoreCase) && !_defaultAllowList.Contains(header.Key, StringComparer.OrdinalIgnoreCase)))
            {
                return new KeyValuePair<string, string>(headerKey, scrubValue);
            }

            return new KeyValuePair<string, string>(headerKey, header.Value);
        }
    }
}
