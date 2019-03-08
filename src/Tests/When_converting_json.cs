using System;
using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Castle.Messages.Requests;
using Castle.Messages.Responses;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_converting_json
    {
        #region General

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

            var result = JsonConvertForCastle.DeserializeObject<GenericObject>(json);

            result.UserId.Should().Be("123");
        }

        [Fact]
        public void Should_deserialize_to_default_if_error()
        {
            var result = JsonConvertForCastle.DeserializeObject<VoidResponse>("");

            result.Should().NotBeNull();
        }

        private class GenericObject
        {
            public string UserId { get; set; }
        }

        #endregion

        #region Domain

        // Null values are skipped by Newtonsoft.Json, so we don't test those
        [Theory]
        [InlineData("non-empty", "\"client_id\":\"non-empty\"")]
        [InlineData("", "\"client_id\":false")]
        public void Should_serialize_client_id_to_false_if_empty(string value, string expected)
        {
            var obj = new RequestContext() { ClientId = value };

            var result = JsonConvertForCastle.SerializeObject(obj);

            result.Should().Contain(expected);
        }

        [Theory]
        [InlineData("Cookie", "true", "\"Cookie\":true")]
        [InlineData("cookie", "non-empty", "\"cookie\":\"non-empty\"")]
        public void Should_serialize_header_to_truebool_if_truestring(
            string name,
            string value, 
            string expected)
        {
            var obj = new RequestContext()
            {
                Headers = new Dictionary<string, string>()
                {
                    [name] = value
                }
            };
        

        var result = JsonConvertForCastle.SerializeObject(obj);

            result.Should().Contain(expected);
        }

        #endregion  
       
    }
}
