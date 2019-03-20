﻿using System.Collections.Generic;
using AutoFixture.Xunit2;
using Castle;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
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
    }
}
