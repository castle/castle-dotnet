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
        public async Task Should_prepare_request_for_send(
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

            requestArg.Should().NotBe(request);
        }
    }
}
