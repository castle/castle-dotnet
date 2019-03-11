using System;
using System.Threading.Tasks;
using Castle.Actions;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Requests;
using Castle.Messages.Responses;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_authenticating
    {
        [Theory, AutoFakeData]
        public async Task Should_return_response_if_successful(
            ActionRequest request,
            CastleConfiguration configuration,
            Verdict response,
            ILogger logger)
        {
            Task<Verdict> Send(ActionRequest req) => Task.FromResult(response);

            var result = await Authenticate.Execute(Send, request, configuration, logger);

            result.Should().Be(response);
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_timeout(
            ActionRequest request,
            string requestUri,
            CastleConfiguration configuration,
            ILogger logger)
        {
            configuration.FailOverStrategy = ActionType.Allow;

            Task<Verdict> Send(ActionRequest req) => throw new CastleTimeoutException(requestUri, configuration.Timeout);

            var result = await Authenticate.Execute(Send, request, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_any_exception(
            ActionRequest request,
            Exception exception,
            CastleConfiguration configuration,
            ILogger logger)
        {
            configuration.FailOverStrategy = ActionType.Allow;

            Task<Verdict> Send(ActionRequest req) => throw exception;

            var result = await Authenticate.Execute(Send, request, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("server error");
        }

        [Theory, AutoFakeData]
        public async Task Should_log_failover_exception_as_warning(
            ActionRequest request,
            Exception exception,
            CastleConfiguration configuration,
            ILogger logger)
        {
            configuration.FailOverStrategy = ActionType.Allow;

            Task<Verdict> Send(ActionRequest req) => throw exception;

            await Authenticate.Execute(Send, request, configuration, logger);

            logger.Received().Warn("Failover, " + exception.ToString());
        }

        [Theory, AutoFakeData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(
            ActionRequest request,
            Exception exception,
            CastleConfiguration configuration,
            ILogger logger)
        {
            configuration.FailOverStrategy = ActionType.None;

            Task<Verdict> Send(ActionRequest req) => throw exception;

            Func<Task> act = async () => await Authenticate.Execute(Send, request, configuration, logger);

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoFakeData]
        public async Task Should_prepare_request_for_send(
            ActionRequest request,
            CastleConfiguration configuration,
            Verdict response,
            ILogger logger)
        {
            ActionRequest requestArg = null;
            Task<Verdict> Send(ActionRequest req)
            {
                requestArg = req;
                return Task.FromResult(response);
            }

            await Authenticate.Execute(Send, request, configuration, logger);

            requestArg.Should().NotBe(request); 
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_do_not_track_is_set(
            ActionRequest request,
            CastleConfiguration configuration,
            Verdict response,
            ILogger logger)
        {
            configuration.DoNotTrack = true;
            configuration.FailOverStrategy = ActionType.Allow;

            Task<Verdict> Send(ActionRequest req) => Task.FromResult(response);

            var result = await Authenticate.Execute(Send, request, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("do not track");
        }
    }
}
