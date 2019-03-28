using System;
using System.Threading.Tasks;
using Castle.Actions;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_authenticating
    {
        [Theory, AutoFakeData]
        public async Task Should_return_response_if_successful(
            CastleConfiguration configuration,
            Verdict response)
        {
            Task<Verdict> Send() => Task.FromResult(response);
            var logger = Substitute.For<IInternalLogger>();

            var result = await Authenticate.Execute(Send, configuration, logger);

            result.Should().Be(response);
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_timeout(
            string requestUri,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<Verdict> Send() => throw new CastleTimeoutException(requestUri, configuration.Timeout);

            var result = await Authenticate.Execute(Send, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_any_exception(
            Exception exception,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<Verdict> Send() => throw exception;

            var result = await Authenticate.Execute(Send, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("server error");
        }

        [Theory, AutoFakeData]
        public async Task Should_log_failover_exception_as_warning(
            Exception exception,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<Verdict> Send() => throw exception;

            await Authenticate.Execute(Send, configuration, logger);

            logger.Received().Warn(Arg.Is<Func<string>>(x => x() == "Failover, " + exception));
        }

        [Theory, AutoFakeData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(
            Exception exception,
            CastleConfiguration configuration)
        {
            configuration.FailOverStrategy = ActionType.None;
            var logger = Substitute.For<IInternalLogger>();

            Task<Verdict> Send() => throw exception;

            Func<Task> act = async () => await Authenticate.Execute(Send, configuration, logger);

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoFakeData]
        public async Task Should_return_failover_response_if_do_not_track_is_set(
            CastleConfiguration configuration,
            Verdict response)
        {
            configuration.DoNotTrack = true;
            configuration.FailOverStrategy = ActionType.Allow;
            var logger = Substitute.For<IInternalLogger>();

            Task<Verdict> Send() => Task.FromResult(response);

            var result = await Authenticate.Execute(Send, configuration, logger);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("do not track");
        }
    }
}
