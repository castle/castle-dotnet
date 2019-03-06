using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Net.Actions;
using Castle.Net.Infrastructure;
using Castle.Net.Infrastructure.Exceptions;
using Castle.Net.Messages;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Tests
{
    public class When_authenticating
    {
        [Theory, AutoData]
        public async Task Should_return_response_if_successful(
            AuthenticateRequest request,
            ActionType failoverStrategy,
            AuthenticateResponse response)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Returns(response);

            var result = await Authenticate.Execute(sender, request, failoverStrategy);

            result.Should().Be(response);
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_timeout(
            AuthenticateRequest request,
            string requestUri,
            int timeout)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new CastleTimeoutException(requestUri, timeout));

            var result = await Authenticate.Execute(sender, request, ActionType.Allow);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_any_exception(
            AuthenticateRequest request)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new Exception("error!"));

            var result = await Authenticate.Execute(sender, request, ActionType.Allow);

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("server error");
        }

        [Theory, AutoData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(
            AuthenticateRequest request)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new Exception("error!"));

            Func<Task> act = async () => await Authenticate.Execute(sender, request, ActionType.None);

            await act.Should().ThrowAsync<CastleExternalException>();
        }
    }
}
