using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Castle.Messages.Requests;
using FluentAssertions;
using Xunit;

namespace Tests.Json
{
    public class When_serializing_special_properties
    {
        // Null values are skipped by Newtonsoft.Json, so we don't test those
        [Theory]
        [InlineData("non-empty", "\"client_id\":\"non-empty\"")]
        [InlineData("", "\"client_id\":false")]
        public void Should_serialize_client_id_to_false_if_empty(string value, string expected)
        {
            var obj = new RequestContext() { ClientId = value };

            var result = JsonForCastle.SerializeObject(obj);

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
        

        var result = JsonForCastle.SerializeObject(obj);

            result.Should().Contain(expected);
        }
    }
}
