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
            var result = request.PrepareApiCopy(options.AllowList, options.DenyList);

            result.Headers.Should().NotBeSameAs(request.Headers);
        }

        [Theory, AutoFakeData]
        public void Should_set_sent_date(ActionRequest request, CastleConfiguration options)
        {
            var result = request.PrepareApiCopy(options.AllowList, options.DenyList);

            result.SentAt.Should().BeAfter(DateTime.MinValue);
        }

        [Theory, AutoFakeData]
        public void Should_set_null_fingerprint_to_empty(ActionRequest request, CastleConfiguration options)
        {
            request.Fingerprint = null;

            var result = request.PrepareApiCopy(options.AllowList, options.DenyList);

            result.Fingerprint.Should().Be("");
        }

        [Theory, AutoFakeData]
        public void Should_preserve_valid_fingerprint(ActionRequest request, CastleConfiguration options)
        {
            var result = request.PrepareApiCopy(options.AllowList, options.DenyList);

            result.Fingerprint.Should().Be(request.Fingerprint);
        }
    }
}
