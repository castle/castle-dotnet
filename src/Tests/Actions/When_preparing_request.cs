using System;
using Castle.Config;
using Castle.Messages.Requests;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests.Actions
{
    public class When_preparing_request
    {
        [Theory, AutoFakeData]
        public void Should_scrub_headers(ActionRequest request, CastleConfiguration options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.Headers.Should().NotBeSameAs(request.Context.Headers);
        }

        [Theory, AutoFakeData]
        public void Should_set_sent_date(ActionRequest request, CastleConfiguration options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.SentAt.Should().BeAfter(DateTime.MinValue);
        }

        [Theory, AutoFakeData]
        public void Should_set_null_clientid_to_empty(ActionRequest request, CastleConfiguration options)
        {
            request.Context.ClientId = null;

            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.ClientId.Should().Be("");
        }

        [Theory, AutoFakeData]
        public void Should_preserve_valid_clientid(ActionRequest request, CastleConfiguration options)
        {
            var result = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

            result.Context.ClientId.Should().Be(request.Context.ClientId);
        }
    }
}
