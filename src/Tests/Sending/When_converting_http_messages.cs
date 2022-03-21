using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Infrastructure.Exceptions;
using Castle.Infrastructure.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
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
            try
            {
                var result = await response.ToCastleException(uri);
            }
            catch (Exception e)
            {
                Assert.True(e is CastleInternalException);
            }

        }

        [Theory, AutoFakeData]
        public void Should_create_exception_from_httpresponse_not_found(HttpResponseMessage response, string uri)
        {

            response.StatusCode = HttpStatusCode.NotFound;
            Func<Task> act = async () => await response.ToCastleException(uri);
            act.Should().Throw<CastleClientErrorException>();
        }

        [Theory, AutoFakeData]
        public void Should_create_exception_from_httpresponse_invalid_token(HttpResponseMessage response, string uri)
        {
            response.StatusCode = (HttpStatusCode)422;
            response.Content = new StringContent("{'type': 'invalid_request_token','message': 'the token is not valid'}", Encoding.UTF8, "application/json");

            Func<Task> act = async () => await response.ToCastleException(uri);
            act.Should().Throw<CastleInvalidTokenException>();

        }

               [Theory, AutoFakeData]
        public void Should_create_exception_from_httpresponse_invalid_parameters(HttpResponseMessage response, string uri)
        {
            response.StatusCode = (HttpStatusCode)422;
            response.Content = new StringContent("{'message': 'parameters are invalid'}", Encoding.UTF8, "application/json");

            Func<Task> act = async () => await response.ToCastleException(uri);
            act.Should().Throw<CastleInvalidParametersException>();
        }
    }
}
