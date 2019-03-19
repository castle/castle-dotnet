using System;

namespace Castle.Infrastructure
{
    internal static class QueryStringBuilder
    {
        public static string Append(string url, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
                return url;

            var prefix = url.Contains("?") ? "&" : "?";
            return $"{url}{prefix}{key}={value}";
        }
    }
}
