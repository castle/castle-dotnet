using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Messages.Requests;

namespace Castle
{
    public static class Context
    {
#if NET461
        public static RequestContext FromHttpRequest(System.Web.HttpRequestBase request, string ipHeader = null)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in request.Headers.Keys)
            {
                headers.Add(key, request.Headers[key]);
            }

            var clientId = request.Headers.AllKeys.Contains("X-Castle-Client-ID", StringComparer.OrdinalIgnoreCase)
                ? request.Headers["X-Castle-Client-ID"]
                : request.Cookies["__cid"]?.Value ?? "";

            var ip = ipHeader != null && request.Headers.AllKeys.Contains(ipHeader, StringComparer.OrdinalIgnoreCase)
                ? request.Headers[ipHeader]
                : request.UserHostAddress;

            return new RequestContext()
            {
                ClientId = clientId,
                Headers = headers,
                Ip = ip
            };
        }
#endif

#if NETSTANDARD2_0
        public static RequestContext FromHttpRequest(Microsoft.AspNetCore.Http.HttpRequest request, string ipHeader = null)
        {
            return new RequestContext()
            {
                ClientId = GetClientId(request.Headers, request.Cookies),
                Headers = request.Headers.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault()),
                Ip = GetIp(() => request.HttpContext.Connection.RemoteIpAddress.ToString(), request.Headers, ipHeader)
            };
        }

        internal static string GetClientId(
            IDictionary<string, Microsoft.Extensions.Primitives.StringValues> headers,
            Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
        {
            return headers.TryGetValue("X-Castle-Client-ID", out var headerId)
                ? headerId.First()
                : cookies["__cid"] ?? "";
        }

        internal static string GetIp(
            Func<string> getIpFromHttpContext,
            IDictionary<string, Microsoft.Extensions.Primitives.StringValues> headers,
            string ipHeader)
        {
            return ipHeader != null && headers.TryGetValue(ipHeader, out var headerValues) 
                ? headerValues.First() 
                : getIpFromHttpContext();
        }
#endif
    }
}
