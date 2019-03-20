using System.Collections.Specialized;
using AutoFixture.Xunit2;
using Castle;
using FluentAssertions;
using Xunit;

namespace Tests.Messages
{
    public class When_creating_request_context_for_Framework
    {
        [Theory, AutoData]
        public void Should_get_client_id_from_castle_header_if_present(
            string castleHeaderValue,
            string cookieValue)
        {
            var headers = new NameValueCollection
            {
                ["X-Castle-Client-ID"] = castleHeaderValue
            };

            string GetCookie(string name) => name == "__cid" ? cookieValue : null;

            var result = Context.GetClientIdForFramework(headers, GetCookie);

            result.Should().Be(castleHeaderValue);
        }

        [Theory, AutoData]
        public void Should_get_client_id_from_cookie_if_castle_header_not_present(
            string otherHeader,
            string otherHeaderValue,
            string cookieValue)
        {
            var headers = new NameValueCollection
            {
                [otherHeader] = otherHeaderValue
            };

            string GetCookie(string name) => name == "__cid" ? cookieValue : null;

            var result = Context.GetClientIdForFramework(headers, GetCookie);

            result.Should().Be(cookieValue);
        }

        [Theory, AutoData]
        public void Should_use_empty_string_if_unable_to_get_client_id(
            string otherHeader,
            string otherHeaderValue,
            string otherCookie,
            string otherCookieValue)
        {
            var headers = new NameValueCollection
            {
                [otherHeader] = otherHeaderValue
            };

            string GetCookie(string name) => name == otherCookie ? otherCookieValue : null;

            var result = Context.GetClientIdForFramework(headers, GetCookie);

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
            var headers = new NameValueCollection
            {
                [ipHeader] = ip,
                [secondaryIpHeader] = secondaryIp,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForFramework(headers, new [] {  ipHeader, secondaryIpHeader }, () => httpContextIp);

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
            var headers = new NameValueCollection
            {
                [secondaryIpHeader] = secondaryIp,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForFramework(headers, new[] { ipHeader, secondaryIpHeader }, () => httpContextIp);

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
            var headers = new NameValueCollection
            {
                [ipHeader] = ip,
                [otherHeader] = otherHeaderValue
            };

            var result = Context.GetIpForFramework(headers, null, () => httpContextIp);

            result.Should().Be(httpContextIp);
        }
    }
}
