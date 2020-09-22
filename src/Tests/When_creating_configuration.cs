using System;
using Castle.Config;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_creating_configuration
    {
        [Fact]
        public void Should_be_able_to_get_recommended_allowlist()
        {
            var result = Castle.Headers.AllowList;

            result.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_throw_exception_if_null_or_empty_secret(string secret)
        {
            Action act = () => new CastleConfiguration(secret);

            act.Should().Throw<ArgumentException>();
        }

        [Theory, AutoFakeData]
        public void Should_be_able_to_set_loglevel(CastleConfiguration configuration)
        {
            configuration.LogLevel = LogLevel.None;
            CastleConfiguration.SetConfiguration(configuration);

            var result = configuration.LogLevel;
            result.Should().Be(LogLevel.None);
        }

        [Theory, AutoFakeData]
        public void Should_be_able_to_set_timeout(CastleConfiguration configuration, int timeout)
        {
            configuration.LogLevel = LogLevel.None;
            CastleConfiguration.SetConfiguration(configuration);

            var result = configuration.LogLevel;
            result.Should().Be(LogLevel.None);
        }

        [Theory, AutoFakeData]
        public void Should_be_able_to_set_baseurl(CastleConfiguration configuration, string baseUrl)
        {
            configuration.BaseUrl = baseUrl;
            CastleConfiguration.SetConfiguration(configuration);

            var result = configuration.BaseUrl;
            result.Should().Be(baseUrl);
        }

        [Theory, AutoFakeData]
        public void Should_be_able_to_set_denylist(CastleConfiguration configuration, string[] denyList)
        {
            configuration.DenyList = denyList;
            CastleConfiguration.SetConfiguration(configuration);

            var result = configuration.DenyList;
            result.Should().BeEquivalentTo(denyList);
        }

        [Theory, AutoFakeData]
        public void Should_be_able_to_set_trustedproxies(CastleConfiguration configuration, string[] trustedProxies)
        {
            configuration.TrustedProxies = trustedProxies;
            CastleConfiguration.SetConfiguration(configuration);

            var result = configuration.TrustedProxies;
            result.Should().BeEquivalentTo(trustedProxies);
        }
    }
}
