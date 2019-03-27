using System;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages.Responses;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests.Sending
{
    public class When_sending_requests
    {
        [Theory, AutoFakeData(typeof(HttpMessageHandlerSuccessCustomization))]
        public async Task Should_log_request_message(
            CastleConfiguration configuration,
            HttpMessageHandler handler)
        {
            var logger = Substitute.For<IInternalLogger>();
            var sut = new HttpMessageSender(configuration, logger, handler);

            foreach (var testMethod in TestMethods)
            {
                var result = await testMethod(sut);
                logger
                    .Received()
                    .Info(Arg.Is<Func<string>>(func => func()
                        .StartsWith("Request")));
            }
        }

        [Theory, AutoFakeData(typeof(HttpMessageHandlerSuccessCustomization))]
        public async Task Should_log_response_message(
            CastleConfiguration configuration,
            HttpMessageHandler handler)
        {
            var logger = Substitute.For<IInternalLogger>();
            var sut = new HttpMessageSender(configuration, logger, handler);

            foreach (var testMethod in TestMethods)
            {
                var result = await testMethod(sut);
                logger
                    .Received()
                    .Info(Arg.Is<Func<string>>(func => func()
                        .StartsWith("Response")));
            }            
        }

        [Theory, AutoFakeData(typeof(HttpMessageHandlerSuccessCustomization))]
        public async Task Should_return_response_if_success_code(
            CastleConfiguration configuration,
            HttpMessageHandler handler)
        {
            var logger = Substitute.For<IInternalLogger>();
            var sut = new HttpMessageSender(configuration, logger, handler);

            foreach (var testMethod in TestMethods)
            {
                var result = await testMethod(sut);
                result.Should().BeOfType<VoidResponse>();
            }            
        }

        [Theory, AutoFakeData(typeof(HttpMessageHandlerFailureCustomization))]
        public void Should_throw_castle_exception_if_not_success_code(
            CastleConfiguration configuration,
            HttpMessageHandler handler)
        {
            var logger = Substitute.For<IInternalLogger>();
            var sut = new HttpMessageSender(configuration, logger, handler);

            foreach (var testMethod in TestMethods)
            {
                Func<Task> act = async () => await testMethod(sut);
                act.Should().Throw<CastleInternalException>();
            }            
        }

        [Theory, AutoFakeData(typeof(HttpMessageHandlerCancelledCustomization))]
        public void Should_throw_timeout_exception_if_operation_cancelled(
            CastleConfiguration configuration,
            HttpMessageHandler handler)
        {
            var logger = Substitute.For<IInternalLogger>();
            var sut = new HttpMessageSender(configuration, logger, handler);

            foreach (var testMethod in TestMethods)
            {
                Func<Task> act = async () => await testMethod(sut);
                act.Should().Throw<CastleTimeoutException>();
            }            
        }

        private static readonly Func<HttpMessageSender, Task<VoidResponse>>[] TestMethods =
        {
            async sender => await sender.Get<VoidResponse>(""),
            async sender => await sender.Post<VoidResponse>("", new { }),
            async sender => await sender.Put<VoidResponse>(""),
            async sender => await sender.Delete<VoidResponse>("", new { })
        };

    }
}
