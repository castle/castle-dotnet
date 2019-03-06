using System;
using Castle.Net.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_serializing_to_json
    {
        [Fact]
        public void Should_write_lowercase_properties()
        {
            const string expected = "{\"property\":\"value\"}";

            var result = JsonConvertForCastle.SerializeObject(new
            {
                Property = "value"
            });

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_ignore_null_properties()
        {
            const string expected = "{\"property\":\"value\"}";

            var result = JsonConvertForCastle.SerializeObject(new
            {
                Property = "value",
                NullProperty = (string) null
            });

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_write_datetime_as_ISO_8601()
        {
            const string expected = "{\"date\":\"1997-08-29T00:00:00Z\"}";

            var date = new DateTime(1997, 8, 29);

            var result = JsonConvertForCastle.SerializeObject(new {date});

            result.Should().Be(expected);
        }
    }
}
