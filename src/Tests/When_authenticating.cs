using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Actions;
using Castle.Config;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.SdkRequests;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_authenticating
    {
        [Theory, AutoData]
        public async Task Should_return_response_if_successful(
            ActionRequest request,
            CastleOptions options,
            Verdict response)
        {
            Task<Verdict> Send(ActionRequest req) => Task.FromResult(response);

            var result = await Authenticate.Execute(Send, request, options);

            result.Should().Be(response);
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_timeout(
            ActionRequest request,
            string requestUri,
            int timeout)
        {
            Task<Verdict> Send(ActionRequest req) => throw new CastleTimeoutException(requestUri, timeout);

            var result = await Authenticate.Execute(Send, request, new CastleOptions() { FailOverStrategy = ActionType.Allow });

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_any_exception(ActionRequest request)
        {
            Task<Verdict> Send(ActionRequest req) => throw new Exception("error!");

            var result = await Authenticate.Execute(Send, request, new CastleOptions() { FailOverStrategy = ActionType.Allow });

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("server error");
        }

        [Theory, AutoData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(ActionRequest request)
        {
            Task<Verdict> Send(ActionRequest req) => throw new Exception("error!");

            Func<Task> act = async () => await Authenticate.Execute(Send, request, new CastleOptions() { FailOverStrategy = ActionType.None });

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoData]
        public async Task Should_scrub_headers(
            ActionRequest request,
            CastleOptions options,
            Verdict response)
        {
            ActionRequest requestArg = null;
            Task<Verdict> Send(ActionRequest req)
            {
                requestArg = req;
                return Task.FromResult(response);
            }

            await Authenticate.Execute(Send, request, options);

            requestArg.Context.Headers.Should().NotEqual(request.Context.Headers); 
        }
    }
}
