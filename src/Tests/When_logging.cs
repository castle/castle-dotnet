using System;
using AutoFixture.Xunit2;
using Castle.Config;
using Castle.Infrastructure;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_logging
    {
        [Theory, AutoData]
        public void Should_handle_receiving_null_logger(
            LogLevel level,
            string message)
        {
            Action act = () => new LoggerWithLevel(null, level).Error(message);

            act.Should().NotThrow();
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        [InlineAutoFakeData(LogLevel.Warn)]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_errors_if_loglevel_is_error_or_higher(
            [Frozen] LogLevel level,
            string message,
            ILogger logger)
        {
            new LoggerWithLevel(logger, level).Error(message);

            logger.Received().Error(message);
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Warn)]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_warnings_if_loglevel_is_warn_or_higher(
            [Frozen] LogLevel level,
            string message,
            ILogger logger)
        {
            new LoggerWithLevel(logger, level).Warn(message);

            logger.Received().Warn(message);
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        public void Should_not_log_warnings_if_loglevel_is_error(
            [Frozen] LogLevel level,
            string message,
            ILogger logger)
        {
            new LoggerWithLevel(logger, level).Warn(message);

            logger.DidNotReceive().Warn(message);
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_info_if_loglevel_is_info(
            [Frozen] LogLevel level,
            string message,
            ILogger logger)
        {
            new LoggerWithLevel(logger, level).Info(message);

            logger.Received().Info(message);
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        [InlineAutoFakeData(LogLevel.Warn)]
        public void Should_not_log_info_if_loglevel_is_warn_or_lower(
            [Frozen] LogLevel level,
            string message,
            ILogger logger)
        {
            new LoggerWithLevel(logger, level).Info(message);

            logger.DidNotReceive().Info(message);
        }
    }
}
