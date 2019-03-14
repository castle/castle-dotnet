using System;
using System.Collections.Generic;
using Castle.Infrastructure.Json;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests.Json
{
    public class When_serializing
    {
        [Theory, MemberData(nameof(TestCases))]
        public void Should_serialize_lowercase_properties(
            Func<object, object> expectedMapper, 
            Func<object, object> converter)
        {
            var expected = expectedMapper("{\"property\":\"value\"}");

            var result = converter(new {
                Property = "value"
            });

            result.Should().BeEquivalentTo(expected);
        }

        [Theory, MemberData(nameof(TestCases))]
        public void Should_not_serialize_null_properties(
            Func<object, object> expectedMapper,
            Func<object, object> converter)
        {
            var expected = expectedMapper("{\"property\":\"value\"}");

            var result = converter(new
            {
                Property = "value",
                NullProperty = (string) null
            });

            result.Should().BeEquivalentTo(expected);
        }

        [Theory, MemberData(nameof(TestCases))]
        public void Should_serialize_datetime_as_ISO_8601(
            Func<object, object> expectedMapper,
            Func<object, object> converter)
        {
            var expected = expectedMapper("{\"date\":\"1997-08-29T00:00:00Z\"}");

            var date = new DateTime(1997, 8, 29);

            var result = converter(new {date});

            result.Should().BeEquivalentTo(expected);
        }

        [Theory, MemberData(nameof(TestCases))]
        public void Should_serialize_to_snakecase(
            Func<object, object> expectedMapper,
            Func<object, object> converter)
        {
            var expected = expectedMapper("{\"user_id\":\"123\"}");

            var result = converter(new {UserId = "123"});

            result.Should().BeEquivalentTo(expected);
        }

        // We want to test both to string and to JObject
        public static IEnumerable<object[]> TestCases =>
            new List<object[]>()
            {
                new object[]
                    {new Func<object, object>(exp => exp), new Func<object, object>(JsonForCastle.SerializeObject)},
                new object[]
                    {new Func<object, object>(exp => JObject.Parse(exp.ToString())), new Func<object, object>(JsonForCastle.FromObject)},
            };
    }
}
