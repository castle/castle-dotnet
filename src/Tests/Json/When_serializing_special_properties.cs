using System;
using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Castle.Messages.Requests;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests.Json
{
    public class When_serializing_special_properties
    {
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

        [Fact]
        public void Should_only_allow_serialization_for_scrub()
        {
            Action act = () => JsonForCastle.DeserializeObject<NotValidScrubbing>("{ \"generic_property\":\"test\" }");
            act.Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Should_only_convert_strings_for_scrub()
        {
           var settings = new JsonSerializerSettings();
           settings.Converters.Add(new StringScrubConverter());

           var result = JsonConvert.SerializeObject(
               new GenericObject() { GenericString = "true", GenericNumber = 2 }, 
               settings);

           result.Should().Be("{\"GenericString\":true,\"GenericNumber\":2}");
        }

        private class GenericObject
        {
            public string GenericString { get; set; }

            public int GenericNumber { get; set; }
        }

        private class NotValidScrubbing
        {
            [JsonConverter(typeof(StringScrubConverter))]
            public string GenericProperty { get; set; }
        }
    }
}
