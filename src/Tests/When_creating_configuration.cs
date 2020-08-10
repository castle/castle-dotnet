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
    }
}
