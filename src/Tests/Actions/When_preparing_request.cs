using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Config;
using Castle.Messages.Requests;
using FluentAssertions;
using Xunit;

namespace Tests.Actions
{
    public class When_preparing_request
    {
        [Theory, AutoData]
        public void Should_scrub_headers(ActionRequest request, CastleOptions options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.Headers.Should().NotEqual(request.Context.Headers);
        }

        [Theory, AutoData]
        public void Should_set_sent_date(ActionRequest request, CastleOptions options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.SentAt.Should().BeAfter(DateTime.MinValue);
        }

        [Theory, AutoData]
        public void Should_set_null_clientid_to_empty(ActionRequest request, CastleOptions options)
        {
            request.Context.ClientId = null;

            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.ClientId.Should().Be("");
        }

        [Theory, AutoData]
        public void Should_preserve_valid_clientid(ActionRequest request, CastleOptions options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.ClientId.Should().Be(request.Context.ClientId);
        }
    }
}
