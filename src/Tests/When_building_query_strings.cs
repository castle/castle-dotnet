using Castle.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_building_query_strings
    {
        [Fact]
        public void Should_append_with_questionmark_if_first_param()
        {
            const string url = "http://test.com";
            const string key = "my_param";
            const string value = "my_value";
            const string expected = "http://test.com?my_param=my_value";

            var result = QueryStringBuilder.Append(url, key, value);

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_append_with_ampersand_if_not_first_param()
        {
            const string url = "http://test.com?other_param=other_value";
            const string key = "my_param";
            const string value = "my_value";
            const string expected = "http://test.com?other_param=other_value&my_param=my_value";

            var result = QueryStringBuilder.Append(url, key, value);

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_not_append_empty_value()
        {
            const string url = "http://test.com";
            const string key = "my_param";
            const string value = null;
            const string expected = "http://test.com";

            var result = QueryStringBuilder.Append(url, key, value);

            result.Should().Be(expected);
        }
    }
}
