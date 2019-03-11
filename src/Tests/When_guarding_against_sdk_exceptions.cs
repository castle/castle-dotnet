using System;
using System.Threading.Tasks;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_guarding_against_sdk_exceptions
    {
        [Theory, AutoFakeData]
        public async Task Should_return_request_response_if_no_exception_is_thrown(
            int response,
            IInternalLogger logger)
        {
            async Task<int> DoRequest()
            {
                return await Task.FromResult(response);
            }

            var result = await ExceptionGuard.Try(DoRequest, logger);

            result.Should().Be(response);
        }

        [Theory, AutoFakeData]
        public async Task Should_rethrow_external_castle_exceptions(
            IInternalLogger logger,
            CastleExternalException exception)
        {
            async Task<int> DoRequest()
            {
                return await Task.FromException<int>(exception);
            }

            Func<Task> act = async () => await ExceptionGuard.Try(DoRequest, logger);

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoFakeData]
        public async Task Should_not_rethrow_non_external_exceptions(
            IInternalLogger logger,
            Exception exception)
        {
            async Task<int> DoRequest()
            {
                return await Task.FromException<int>(exception);
            }

            Func<Task> act = async () => await ExceptionGuard.Try(DoRequest, logger);

            await act.Should().NotThrowAsync<Exception>();
        }

        [Theory, AutoFakeData]
        public async Task Should_log_exceptions_as_errors(
            IInternalLogger logger,
            Exception exception)
        {
            async Task<int> DoRequest()
            {
                return await Task.FromException<int>(exception);
            }

            await ExceptionGuard.Try(DoRequest, logger);

            logger.Received().Error(exception.ToString);
        }

        [Theory, AutoFakeData]
        public async Task Should_return_empty_response_object_if_exception_was_caught(
            IInternalLogger logger,
            Exception exception)
        {
            async Task<int> DoRequest()
            {
                return await Task.FromException<int>(exception);
            }

            var result = await ExceptionGuard.Try(DoRequest, logger);

            result.Should().Be(new int());
        }
    }
}
