using Castle.Config;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_creating_configuration
    {
        [Theory, AutoFakeData]
        public void Should_always_add_Cookie_when_setting_blacklist(CastleConfiguration configuration, string[] newList)
        {
            configuration.Blacklist = newList;

            configuration.Blacklist.Should().Contain("Cookie");
        }

        [Theory, AutoFakeData]
        public void Should_preserve_Cookie_when_setting_blacklist_to_null(CastleConfiguration configuration)
        {
            configuration.Blacklist = null;

            configuration.Blacklist.Should().Contain("Cookie");
        }

        [Fact]
        public void Should_be_able_to_get_recommended_whitelist()
        {
            var result = Castle.Headers.Whitelist;

            result.Should().NotBeEmpty();
        }
    }
}
