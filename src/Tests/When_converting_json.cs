using System;
using Castle.Infrastructure;
using Castle.Messages;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_converting_json
    {
        [Fact]
        public void Should_serialize_lowercase_properties()
        {
            const string expected = "{\"property\":\"value\"}";

            var result = JsonConvertForCastle.SerializeObject(new
            {
                Property = "value"
            });

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_not_serialize_null_properties()
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
        public void Should_serialize_datetime_as_ISO_8601()
        {
            const string expected = "{\"date\":\"1997-08-29T00:00:00Z\"}";

            var date = new DateTime(1997, 8, 29);

            var result = JsonConvertForCastle.SerializeObject(new {date});

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_serialize_to_snakecase()
        {
            const string expected = "{\"user_id\":\"123\"}";

            var result = JsonConvertForCastle.SerializeObject(new {UserId = "123"});

            result.Should().Be(expected);
        }

        [Fact]
        public void Should_deserialize_from_snakecase()
        {
            const string json = "{\"user_id\":\"123\"}";

            var result = JsonConvertForCastle.DeserializeObject<TestObject>(json);

            result.UserId.Should().Be("123");
        }

        [Fact]
        public void Should_deserialize_to_default_if_error()
        {
            var result = JsonConvertForCastle.DeserializeObject<VoidResponse>("");

            result.Should().NotBeNull();
        }

        private class TestObject
        {
            public string UserId { get; set; }
        }
    }
}
