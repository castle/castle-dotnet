using System.Collections.Generic;
using AutoFixture.Xunit2;
using Castle;
using FluentAssertions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace Tests
{
    public class When_creating_request_context
    {
        [Theory, AutoData]
        public void Should_get_client_id_from_castle_header_if_present(
            string castleHeaderValue,
            string otherHeader,
            string otherHeaderValue,
            string cookieValue)
        {
            var headers = new Dictionary<string, StringValues>()
            {
                ["X-Castle-Client-ID"] = castleHeaderValue,
                [otherHeader] = otherHeaderValue
            };

            var cookies = new RequestCookieCollection(new Dictionary<string, string>()
            {
                ["__cid"] = cookieValue
            });

            var result = Context.GetClientId(headers, cookies);

            result.Should().Be(castleHeaderValue);
        }
    }
}
