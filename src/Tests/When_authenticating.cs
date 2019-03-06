using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Net.Actions;
using Castle.Net.Config;
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
            ActionRequest request,
            CastleOptions options,
            AuthenticateResponse response)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Returns(response);

            var result = await Authenticate.Execute(sender, request, options);

            result.Should().Be(response);
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_timeout(
            ActionRequest request,
            string requestUri,
            int timeout)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new CastleTimeoutException(requestUri, timeout));

            var result = await Authenticate.Execute(sender, request, new CastleOptions() { FailOverStrategy = ActionType.Allow });

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("timeout");
        }

        [Theory, AutoData]
        public async Task Should_return_failover_response_if_any_exception(ActionRequest request)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new Exception("error!"));

            var result = await Authenticate.Execute(sender, request, new CastleOptions() { FailOverStrategy = ActionType.Allow });

            result.Failover.Should().Be(true);
            result.FailoverReason.Should().Be("server error");
        }

        [Theory, AutoData]
        public async Task Should_throw_exception_if_failing_over_with_no_strategy(ActionRequest request)
        {
            var sender = Substitute.For<IMessageSender>();
            sender.Post<AuthenticateResponse>(request, Arg.Any<string>()).Throws(new Exception("error!"));

            Func<Task> act = async () => await Authenticate.Execute(sender, request, new CastleOptions() { FailOverStrategy = ActionType.None });

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoData]
        public async Task Should_scrub_headers(
            ActionRequest request,
            CastleOptions options)
        {
            var sender = Substitute.For<IMessageSender>();

            await Authenticate.Execute(sender, request, options);

            await sender.Received().Post<AuthenticateResponse>(
                Arg.Is<ActionRequest>(match => match.Context.Headers != request.Context.Headers),
                Arg.Any<string>());

            
        }
    }
}
