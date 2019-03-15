namespace Castle
{
    /// <summary>
    /// Recommended request context headers
    /// </summary>
    public static class Headers
    {
        public static readonly string[] Whitelist = new[]
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
            "Cookie",
            "Host",
            "Origin",
            "Pragma",
            "Referer",
            "User-Agent",
        };
    }
}
