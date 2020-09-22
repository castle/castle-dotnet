using System.Collections.Generic;
using AutoFixture.Xunit2;
using Castle;
using Castle.Config;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Tests.SetUp;
using Xunit;

namespace Tests.Messages
{
    public class When_creating_request_context_for_Core
    {
        [Theory, AutoData]
        public void Should_get_client_id_from_castle_header_if_present(
            string castleHeaderValue,
            string cookieValue)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                ["X-Castle-Client-ID"] = castleHeaderValue,
            };

            var cookies = new RequestCookieCollection(new Dictionary<string, string>()
            {
                ["__cid"] = cookieValue
            });

            var result = Context.GetClientIdForCore(headers, cookies);

            result.Should().Be(castleHeaderValue);
        }

        [Theory, AutoData]
        public void Should_get_client_id_from_cookie_if_castle_header_not_present(
            string otherHeader,
            string otherHeaderValue,
            string cookieValue)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [otherHeader] = otherHeaderValue
            };

            var cookies = new RequestCookieCollection(new Dictionary<string, string>()
            {
                ["__cid"] = cookieValue
            });

            var result = Context.GetClientIdForCore(headers, cookies);

            result.Should().Be(cookieValue);
        }

        [Theory, AutoData]
        public void Should_use_empty_string_if_unable_to_get_client_id(
            string otherHeader,
            string otherHeaderValue,
            string otherCookie,
            string otherCookieValue)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [otherHeader] = otherHeaderValue
            };

            var cookies = new RequestCookieCollection(new Dictionary<string, string>()
            {
                [otherCookie] = otherCookieValue
            });

            var result = Context.GetClientIdForCore(headers, cookies);

            result.Should().Be("");
        }

        [Theory, AutoData]
        public void Should_get_ip_from_supplied_headers_in_order(
            string ipHeader,
            string ip,
            string secondaryIpHeader,
            string secondaryIp,
            string otherHeader,
            string otherHeaderValue,
            string httpContextIp)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [ipHeader] = ip,
                [secondaryIpHeader] = secondaryIp,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForCore(headers, new [] {  ipHeader, secondaryIpHeader }, () => httpContextIp);

            result.Should().Be(ip);
        }

        [Theory, AutoData]
        public void Should_get_ip_from_second_header_if_first_is_not_found(
            string ipHeader,
            string secondaryIpHeader,
            string secondaryIp,
            string otherHeader,
            string otherHeaderValue,
            string httpContextIp)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [secondaryIpHeader] = secondaryIp,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForCore(headers, new[] { ipHeader, secondaryIpHeader }, () => httpContextIp);

            result.Should().Be(secondaryIp);
        }

        [Theory, AutoData]
        public void Should_get_ip_from_httpcontext_if_no_header_supplied(
            string ipHeader,
            string ip,
            string otherHeader,
            string otherHeaderValue,
            string httpContextIp)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [ipHeader] = ip,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForCore(headers, null, () => httpContextIp);

            result.Should().Be(httpContextIp);
        }

        [Theory, AutoData]
        public void Should_get_regular_ip(
            string ipHeader,
            string ip
        )
        {
            var headers = new Dictionary<string, StringValues>()
            {
                [ipHeader] = ip,
            };

            var result = Context.GetIpForCore(headers, null, () => ip);

            result.Should().Be(ip);
        }

        [Theory, AutoData]
        public void Should_get_other_ip_header(string cfConnectiongIp)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                ["Cf-Connecting-Ip"] = cfConnectiongIp,
                ["X-Forwarded-For"] = "1.1.1.1, 1.2.2.2, 1.2.3.5"
            };

            var ipHeaders = new[] {"Cf-Connecting-Ip", "X-Forwarded-For"};

            var result = Context.GetIpForCore(headers, ipHeaders, () => cfConnectiongIp);

            result.Should().Be(cfConnectiongIp);
        }

        [Theory, AutoFakeData]
        public void Should_get_first_available_with_all_trusted_proxies(string defaultIp)
        {
            var headers = new Dictionary<string, StringValues>
            {
                ["Remote-Addr"] = "127.0.0.1",
                ["X-Forwarded-For"] = "127.0.0.1,10.0.0.1,172.31.0.1,192.168.0.1"
            };

            var result = Context.GetIpForCore(headers, null, () => defaultIp);
            result.Should().Be("127.0.0.1");
        }

        [Theory, AutoFakeData]
        public void Should_get_first_available_with_trust_proxy_chain(CastleConfiguration configuration, string defaultIp)
        {
            var headers = new Dictionary<string, StringValues>
            {
                ["Remote-Addr"] = "6.6.6.4",
                ["X-Forwarded-For"] = "6.6.6.6, 2.2.2.3, 6.6.6.5"
            };

            configuration.TrustProxyChain = true;
            CastleConfiguration.SetConfiguration(configuration);

            var result = Context.GetIpForCore(headers, null, () => defaultIp);
            result.Should().Be("6.6.6.6");
        }

        [Theory, AutoFakeData]
        public void Should_get_equivalent_to_trusted_proxy_depth_1(CastleConfiguration configuration, string defaultIp)
        {
            var headers = new Dictionary<string, StringValues>
            {
                ["Remote-Addr"] = "6.6.6.4",
                ["X-Forwarded-For"] = "6.6.6.6, 2.2.2.3, 6.6.6.5"
            };

            configuration.TrustedProxyDepth = 1;
            CastleConfiguration.SetConfiguration(configuration);

            var result = Context.GetIpForCore(headers, null, () => defaultIp);
            result.Should().Be("2.2.2.3");
        }

        [Theory, AutoFakeData]
        public void Should_get_equivalent_to_trusted_proxy_depth_2(CastleConfiguration configuration, string defaultIp)
        {
            var headers = new Dictionary<string, StringValues>
            {
                ["Remote-Addr"] = "6.6.6.4",
                ["X-Forwarded-For"] = "6.6.6.6, 2.2.2.3, 6.6.6.5, 6.6.6.7"
            };

            configuration.TrustedProxyDepth = 2;
            configuration.IpHeaders = new[] {"X-Forwarded-For", "Remote-Addr"};
            CastleConfiguration.SetConfiguration(configuration);

            var result = Context.GetIpForCore(headers, null, () => defaultIp);
            result.Should().Be("2.2.2.3");
        }
    }
}
