using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Castle.Messages.Requests;

namespace Castle
{
    public static class Context
    {
#if NET461 || NET48
        public static RequestContext FromHttpRequest(System.Web.HttpRequestBase request, string[] ipHeaders = null)
        {
            var headers = new Dictionary<string, string>();
            foreach (string key in request.Headers.Keys)
            {
                headers.Add(key, request.Headers[key]);
            }

            var clientId = GetClientIdForFramework(request.Headers, name => request.Cookies[name]?.Value);

            var ip = GetIpForFramework(request.Headers, ipHeaders, () => request.UserHostAddress);

            return new RequestContext()
            {
                ClientId = clientId,
                Headers = headers,
                Ip = ip
            };
        }
#endif

        internal static string GetClientIdForFramework(NameValueCollection headers, Func<string, string> getCookieValue)
        {
            return headers.AllKeys.Contains("X-Castle-Client-ID", StringComparer.OrdinalIgnoreCase)
                ? headers["X-Castle-Client-ID"]
                : getCookieValue("__cid") ?? "";
        }

        internal static string GetIpForFramework(NameValueCollection headers, string[] ipHeaders, Func<string> getIpFromHttpContext)
        {
            foreach (var header in ipHeaders ?? new string[] { })
            {
                if (headers.AllKeys.Contains(header, StringComparer.OrdinalIgnoreCase))
                    return headers[header];
            }

            return getIpFromHttpContext();
        }

#if NETSTANDARD2_0 || NETCOREAPP
        public static RequestContext FromHttpRequest(Microsoft.AspNetCore.Http.HttpRequest request, string[] ipHeaders = null)
        {
            return new RequestContext()
            {
                ClientId = GetClientIdForCore(request.Headers, request.Cookies),
                Headers = request.Headers.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault()),
                Ip = GetIpForCore(request.Headers, ipHeaders, () => request.HttpContext.Connection.RemoteIpAddress?.ToString())
            };
        }

        internal static string GetClientIdForCore(
            IDictionary<string, Microsoft.Extensions.Primitives.StringValues> headers,
            Microsoft.AspNetCore.Http.IRequestCookieCollection cookies)
        {
            return headers.TryGetValue("X-Castle-Client-ID", out var headerId)
                ? headerId.First()
                : cookies["__cid"] ?? "";
        }

        internal static string GetIpForCore(
            IDictionary<string, Microsoft.Extensions.Primitives.StringValues> headers,
            string[] ipHeaders,
            Func<string> getIpFromHttpContext)
        {
            foreach (var header in ipHeaders ?? new string[] {})
            {
                if (headers.TryGetValue(header, out var headerValues))
                    return headerValues.First();
            }

            return getIpFromHttpContext();
        }
#endif
    }
}
