using AutoFixture.Xunit2;
using Castle.Infrastructure.Json;
using Castle.Messages.Responses;
using FluentAssertions;
using Xunit;

namespace Tests.Json
{
    public class When_deserializing
    {      
        [Fact]
        public void Should_deserialize_from_snakecase()
        {
            const string json = "{\"user_id\":\"123\"}";

            var result = JsonForCastle.DeserializeObject<GenericObject>(json);

            result.UserId.Should().Be("123");
        }

        [Fact]
        public void Should_deserialize_to_default_if_error()
        {
            var result = JsonForCastle.DeserializeObject<VoidResponse>("");

            result.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void Should_deserialize_with_json_property_if_appropriate_type(Device obj)
        {
            var json = JsonForCastle.SerializeObject(obj);
            var result = JsonForCastle.DeserializeObject<Device>(json);

            result.Internal.Should().NotBeNull();
        }

        [Theory, AutoData]
        public void Should_not_deserialize_with_json_property_if_not_appropriate_type(DeviceItem obj)
        {
            var json = JsonForCastle.SerializeObject(obj);
            var result = JsonForCastle.DeserializeObject<DeviceItem>(json);

            result.Should().NotBeNull();
        }

        private class GenericObject
        {
            public string UserId { get; set; }
        }
             
    }
}
