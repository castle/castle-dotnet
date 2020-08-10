namespace Castle
{
    /// <summary>
    /// Recommended request context headers
    /// </summary>
    public static class Headers
    {
        public static readonly string[] AllowList = new[]
        {
            "Accept",
            "Accept-Charset",
            "Accept-Datetime",
            "Accept-Encoding",
            "Accept-Language",
            "Cache-Control",
            "Connection",
            "Content-Length",
            "Content-Type",
            "Dnt",
            "Host",
            "Origin",
            "Pragma",
            "Referer",
            "Sec-Fetch-Dest",
            "Sec-Fetch-Mode",
            "Sec-Fetch-Site",
            "Sec-Fetch-User",
            "TE",
            "Upgrade-Insecure-Requests",
            "User-Agent",
            "X-Castle-Client-Id",
        };
    }
}
