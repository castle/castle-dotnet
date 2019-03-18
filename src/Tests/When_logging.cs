using System;
using System.Collections.Generic;
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
        [Theory, MemberData(nameof(NullLoggerMethods))]
        public void Should_handle_errors_without_supplied_logger(Action act)
        {
            act.Should().NotThrow();
        }

        public static IEnumerable<object[]> NullLoggerMethods
        {
            get
            {
                string GetMessage() => "";

                return new List<object[]>()
                {
                    new object[] { new Action(() => new LoggerWithLevels(null, LogLevel.Error).Error(GetMessage)) },
                    new object[] { new Action(() => new LoggerWithLevels(null, LogLevel.Warn).Warn(GetMessage)) },
                    new object[] { new Action(() => new LoggerWithLevels(null, LogLevel.Info).Info(GetMessage)) }
                    };
            }
        }
          
        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        [InlineAutoFakeData(LogLevel.Warn)]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_errors_if_loglevel_is_error_or_higher(
            [Frozen] LogLevel level,
            Func<string> getMessage,
            ICastleLogger logger)
        {
            new LoggerWithLevels(logger, level).Error(getMessage);

            logger.Received().Error(getMessage());
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Warn)]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_warnings_if_loglevel_is_warn_or_higher(
            [Frozen] LogLevel level,
            Func<string> getMessage,
            ICastleLogger logger)
        {
            new LoggerWithLevels(logger, level).Warn(getMessage);

            logger.Received().Warn(getMessage());
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        public void Should_not_log_warnings_if_loglevel_is_error(
            [Frozen] LogLevel level,
            Func<string> getMessage,
            ICastleLogger logger)
        {
            new LoggerWithLevels(logger, level).Warn(getMessage);

            logger.DidNotReceive().Warn(getMessage());
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Info)]
        public void Should_log_info_if_loglevel_is_info(
            [Frozen] LogLevel level,
            Func<string> getMessage,
            ICastleLogger logger)
        {
            new LoggerWithLevels(logger, level).Info(getMessage);

            logger.Received().Info(getMessage());
        }

        [Theory]
        [InlineAutoFakeData(LogLevel.Error)]
        [InlineAutoFakeData(LogLevel.Warn)]
        public void Should_not_log_info_if_loglevel_is_warn_or_lower(
            [Frozen] LogLevel level,
            Func<string> getMessage,
            ICastleLogger logger)
        {
            new LoggerWithLevels(logger, level).Info(getMessage);

            logger.DidNotReceive().Info(getMessage());
        }

        [Theory, AutoFakeData]
        public void Should_not_log_when_level_is_none(
            Func<string> getMessage,
            ICastleLogger castleLogger)
        {
            var logger = new LoggerWithLevels(castleLogger, LogLevel.None);
            logger.Info(getMessage);
            logger.Warn(getMessage);
            logger.Error(getMessage);

            castleLogger.DidNotReceive().Info(getMessage());
        }
    }
}
