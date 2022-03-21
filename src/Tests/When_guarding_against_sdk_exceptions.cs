using System;
using System.Threading.Tasks;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages.Responses;
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
            Verdict response)
        {
            var logger = Substitute.For<IInternalLogger>();
            async Task<Verdict> DoRequest()
            {
                return await Task.FromResult(response);
            }

            var result = await ExceptionGuard.Try(DoRequest, logger);

            result.Should().Be(response);
        }

        [Theory, AutoFakeData]
        public async Task Should_rethrow_external_castle_exceptions(
            CastleExternalException exception)
        {
            var logger = Substitute.For<IInternalLogger>();
            async Task<Verdict> DoRequest()
            {
                return await Task.FromException<Verdict>(exception);
            }

            Func<Task> act = async () => await ExceptionGuard.Try(DoRequest, logger);

            await act.Should().ThrowAsync<CastleExternalException>();
        }

        [Theory, AutoFakeData]
        public async Task Should_not_rethrow_non_external_exceptions(
            Exception exception)
        {
            var logger = Substitute.For<IInternalLogger>();
            async Task<Verdict> DoRequest()
            {
                return await Task.FromException<Verdict>(exception);
            }

            Func<Task> act = async () => await ExceptionGuard.Try(DoRequest, logger);

            await act.Should().NotThrowAsync<Exception>();
        }

        [Theory, AutoFakeData]
        public async Task Should_log_exceptions_as_errors(
            Exception exception)
        {
            var logger = Substitute.For<IInternalLogger>();
            async Task<Verdict> DoRequest()
            {
                return await Task.FromException<Verdict>(exception);
            }

            await ExceptionGuard.Try(DoRequest, logger);

            logger.Received().Error(exception.ToString);
        }

        [Theory, AutoFakeData]
        public async Task Should_return_null_response_if_exception_was_caught(
            Exception exception)
        {
            var logger = Substitute.For<IInternalLogger>();
            async Task<Verdict> DoRequest()
            {
                return await Task.FromException<Verdict>(exception);
            }

            var result = await ExceptionGuard.Try(DoRequest, logger);

            result.Should().BeNull();
        }

        [Theory, AutoFakeData]
        public Task Should_throw_if_exception_of_type_not_found(CastleClientErrorException exception)
        {
            var logger = Substitute.For<IInternalLogger>();

            Func<Task<Verdict>> request = async () => await Task.FromException<Verdict>(exception);
            Func<Task<Verdict>> res = async() => await ExceptionGuard.Try(request, logger);
            res.Should().Throw<CastleClientErrorException>();
            return Task.CompletedTask;
        }

                [Theory, AutoFakeData]
        public Task Should_throw_if_exception_of_type_invalid_parameters(CastleInvalidParametersException exception)
        {
            var logger = Substitute.For<IInternalLogger>();

            Func<Task<Verdict>> request = async () => await Task.FromException<Verdict>(exception);
            Func<Task<Verdict>> res = async() => await ExceptionGuard.Try(request, logger);
            res.Should().Throw<CastleInvalidParametersException>();
            return Task.CompletedTask;
        }

              public Task Should_throw_if_exception_of_type_invalid_token(CastleInvalidTokenException exception)
        {
            var logger = Substitute.For<IInternalLogger>();

            Func<Task<Verdict>> request = async () => await Task.FromException<Verdict>(exception);
            Func<Task<Verdict>> res = async() => await ExceptionGuard.Try(request, logger);
            res.Should().Throw<CastleInvalidTokenException>();
            return Task.CompletedTask;
        }
    }
}
