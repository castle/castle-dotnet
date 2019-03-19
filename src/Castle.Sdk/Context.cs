using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Messages.Requests;

namespace Castle
{
    public static class Context
    {
#if NET461
        public static RequestContext FromHttpRequest(System.Web.HttpRequestBase request)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in request.Headers.Keys)
            {
                headers.Add(key, request.Headers[key]);
            }

            var clientId = request.Headers.AllKeys.Contains("X-Castle-Client-ID", StringComparer.OrdinalIgnoreCase)
                ? request.Headers["X-Castle-Client-ID"]
                : request.Cookies["__cid"]?.Value;

            return new RequestContext()
            {
                ClientId = clientId,
                Headers = headers,
                Ip = request.UserHostAddress
            };
        }
#endif

#if NETSTANDARD2_0
        public static RequestContext FromHttpRequest(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return new RequestContext()
            {
                ClientId = GetClientId(request.Headers, request.Cookies),
                Headers = request.Headers.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault()),
                Ip = request.HttpContext.Connection.RemoteIpAddress.ToString(),
            };
        }

        internal static string GetClientId(
            IDictionary<string, Microsoft.Extensions.Primitives.StringValues> headers,
            Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
        {
            return headers.TryGetValue("X-Castle-Client-ID", out var headerId)
                ? headerId.First()
                : cookies["__cid"];
        }
#endif
    }
}
