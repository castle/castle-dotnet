using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Infrastructure.Extensions;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests.Sending
{
    public class When_converting_http_messages
    {
        [Theory, AutoData]
        public void Should_create_stringcontent_from_payload(object payload)
        {
            var result = payload.ToHttpContent();

            result.ReadAsStringAsync().Result.Should().NotBeNullOrEmpty();
            result.Headers.ContentType.CharSet = "utf-8";
            result.Headers.ContentType.MediaType = "application/json";
        }

        [Theory, AutoFakeData]
        public async Task Should_create_exception_from_httpresponse(HttpResponseMessage response, string uri)
        {
            var result = await response.ToCastleException(uri);

            result.HttpStatusCode.Should().Be(response.StatusCode);
            result.RequestUri.Should().Be(uri);
            result.Message.Should().Be(await response.Content.ReadAsStringAsync());
        }
    }
}
