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
    }
}
