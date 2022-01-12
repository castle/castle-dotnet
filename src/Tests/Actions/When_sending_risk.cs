using System;
using System.Threading.Tasks;
using Castle.Actions;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_sending_risk
    {
        [Theory, AutoFakeData]
        public async Task Should_return_response_if_successful(
            CastleConfiguration configuration, RiskResponse response)
        {
            Task<RiskResponse> Send() => Task.FromResult(response);

            var logger = Substitute.For<IInternalLogger>();

            var result = await Risk.Execute(Send, configuration, logger);

            result.Should().Be(response);
        }


        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_timeout(
            string requestUri,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<RiskResponse> Send() => throw new CastleTimeoutException(requestUri, configuration.Timeout);

            var result = await Risk.Execute(Send, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoFakeData]
        public async Task Should_log_failover_exception_as_warning(
            Exception exception,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<RiskResponse> Send() => throw exception;

            await Risk.Execute(Send, configuration, logger);

            logger.Received().Warn(Arg.Is<Func<string>>(x => x() == "Failover, " + exception));
        }

        [Theory, AutoFakeData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(
            Exception exception,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.None;
            var logger = Substitute.For<IInternalLogger>();

            Task<RiskResponse> Send() => throw exception;

            Func<Task> act = async () => await Risk.Execute(Send, configuration, logger);

            await act.Should().ThrowAsync<CastleExternalException>();
        }

    }
}
