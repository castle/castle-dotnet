using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Actions;
using Castle.Config;
using Castle.Messages.Requests;
using Castle.Messages.Responses;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_tracking
    {
        [Theory, AutoData]
        public async Task Should_scrub_headers(
            ActionRequest request,
            CastleOptions options)
        {
            ActionRequest requestArg = null;
            Task<VoidResponse> Send(ActionRequest req)
            {
                requestArg = req;
                return Task.FromResult(new VoidResponse());
            }

            await Track.Execute(Send, request, options);

            requestArg.Context.Headers.Should().NotEqual(request.Context.Headers);
        }

        [Theory, AutoData]
        public async Task Should_set_sent_date(
            ActionRequest request,
            CastleOptions options)
        {
            ActionRequest requestArg = null;
            Task<VoidResponse> Send(ActionRequest req)
            {
                requestArg = req;
                return Task.FromResult(new VoidResponse());
            }

            await Track.Execute(Send, request, options);

            requestArg.SentAt.Should().BeAfter(DateTime.MinValue);
        }
    }
}
