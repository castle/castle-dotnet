using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Castle.Config;
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
            var cfg = CastleConfiguration.Configuration;

            if (null == ipHeaders || !ipHeaders.Any())
            {
                ipHeaders = cfg.IpHeaders ?? new[] {"X-Forwarded-For", "Remote-Addr"};
            }

            var trustProxyChain = cfg.TrustProxyChain;
            var trustedProxyDepth = cfg.TrustedProxyDepth;
            var trustedProxies = cfg.TrustedProxies;

            string[] LimitProxyDepth(string[] ips, string ipHeader)
            {
                if (new[] {"X-Forwarded-For"}.Contains(ipHeader))
                {
                    return ips.Take(Math.Max(0, ips.Length - trustedProxyDepth)).ToArray();
                }

                return ips;
            }

            string[] IpsFrom(string ipHeader)
            {
                if (!headers.AllKeys.Contains(ipHeader))
                    return new string[] {}.ToArray();

                var value = headers[ipHeader];
                if (0 == value.Length)
                    return new string[] {}.ToArray();

                var ips = value.Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries);

                ips = LimitProxyDepth(ips, ipHeader);
                return ips;
            }

            string RemoveProxies(string[] ips)
            {
                if (trustProxyChain) return ips.FirstOrDefault();
                return ips.LastOrDefault(ip => !IsInternalIpAddress(ip) && !trustedProxies.Contains(ip));
            }

            var ipv = "";
            var allIps = new List<string>();
            foreach (var header in ipHeaders)
            {
                var ipf = IpsFrom(header);
                ipv = RemoveProxies(ipf);

                if (!string.IsNullOrEmpty(ipv))
                {
                    return ipv;
                }
                allIps.AddRange(ipf);
            }

            ipv = allIps.FirstOrDefault();

            if (!string.IsNullOrEmpty(ipv))
            {
                return ipv;
            }

            foreach (var header in ipHeaders)
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
            var cfg = CastleConfiguration.Configuration;

            if (null == ipHeaders || !ipHeaders.Any())
            {
                ipHeaders = cfg.IpHeaders ?? new[] {"X-Forwarded-For", "Remote-Addr"};
            }

            var trustProxyChain = cfg.TrustProxyChain;
            var trustedProxyDepth = cfg.TrustedProxyDepth;
            var trustedProxies = cfg.TrustedProxies;

            string[] LimitProxyDepth(string[] ips, string ipHeader)
            {
                if (new[] {"X-Forwarded-For"}.Contains(ipHeader))
                {
                    return ips.Take(Math.Max(0, ips.Length - trustedProxyDepth)).ToArray();
                }

                return ips;
            }

            string[] IpsFrom(string ipHeader)
            {
                if (!headers.ContainsKey(ipHeader))
                    return new string[] {}.ToArray();

                var value = headers[ipHeader];
                if (0 == value.Count)
                    return new string[] {}.ToArray();

                var ips = value.First().Split(new[] {",", " "}, StringSplitOptions.RemoveEmptyEntries);

                ips = LimitProxyDepth(ips, ipHeader);
                return ips;
            }

            string RemoveProxies(string[] ips)
            {
                if (trustProxyChain) return ips.FirstOrDefault();
                return ips.LastOrDefault(ip => !IsInternalIpAddress(ip) && !trustedProxies.Contains(ip));
            }

            var ipv = "";
            var allIps = new List<string>();
            foreach (var header in ipHeaders)
            {
                var ipf = IpsFrom(header);
                ipv = RemoveProxies(ipf);

                if (!string.IsNullOrEmpty(ipv))
                {
                    return ipv;
                }
                allIps.AddRange(ipf);
            }

            ipv = allIps.FirstOrDefault();

            if (!string.IsNullOrEmpty(ipv))
            {
                return ipv;
            }

            foreach (var header in ipHeaders)
            {
                if (headers.TryGetValue(header, out var headerValues))
                    return headerValues.First();
            }

            return getIpFromHttpContext();
        }
#endif

        private static bool IsInternalIpAddress(string ip)
        {
            var regex = new Regex(
                @"(^127\.0\.0\.1)|(^10\.)|(^172\.1[6-9]\.)|(^172\.2[0-9]\.)|(^172\.3[0-1]\.)|(^192\.168\.)|(^::1)|localhost");

            return regex.Match(ip).Success;
        }
    }
}
