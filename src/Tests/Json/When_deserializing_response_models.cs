using System;
using System.Collections.Generic;
using Castle.Infrastructure.Json;
using Castle.Messages.Responses;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Tests.Json
{
    public class When_deserializing_response_models
    {
        // Every public response model exposes a [JsonExtensionData] AdditionalData bag so
        // that fields added by the API are captured instead of being silently dropped.
        public static IEnumerable<object[]> AllResponseModels => new List<object[]>
        {
            new object[] { typeof(RiskResponse) },
            new object[] { typeof(ScoreItem) },
            new object[] { typeof(DeviceItem) },
            new object[] { typeof(DeviceContext) },
            new object[] { typeof(Policy) },
            new object[] { typeof(Location) },
            new object[] { typeof(UserAgent) },
            new object[] { typeof(ListResponse) },
            new object[] { typeof(ListItemResponse) },
            new object[] { typeof(EventsResponse) },
            new object[] { typeof(EventsSchemaResponse) },
            new object[] { typeof(BatchListItemsResponse) },
            new object[] { typeof(CountResponse) },
        };

        // Top-level responses also implement IHasJson, exposing the full raw payload.
        public static IEnumerable<object[]> HasJsonResponses => new List<object[]>
        {
            new object[] { typeof(RiskResponse) },
            new object[] { typeof(ListResponse) },
            new object[] { typeof(ListItemResponse) },
            new object[] { typeof(EventsResponse) },
            new object[] { typeof(EventsSchemaResponse) },
            new object[] { typeof(BatchListItemsResponse) },
            new object[] { typeof(CountResponse) },
        };

        [Theory, MemberData(nameof(AllResponseModels))]
        public void Should_capture_unknown_fields_in_additional_data(Type type)
        {
            const string json = @"{ ""brand_new_field"": ""surprise"", ""nested_new"": { ""a"": 1 } }";

            var result = Deserialize(type, json);

            var additionalData = GetAdditionalData(type, result);
            additionalData.Should().ContainKey("brand_new_field");
            additionalData["brand_new_field"].ToObject<string>().Should().Be("surprise");
            additionalData.Should().ContainKey("nested_new");
            additionalData["nested_new"]["a"].ToObject<int>().Should().Be(1);
        }

        [Theory, MemberData(nameof(AllResponseModels))]
        public void Should_not_throw_when_all_fields_are_missing(Type type)
        {
            var result = Deserialize(type, "{}");

            result.Should().NotBeNull();
            GetAdditionalData(type, result).Should().BeEmpty();
        }

        [Theory, MemberData(nameof(HasJsonResponses))]
        public void Should_expose_full_raw_payload_via_internal(Type type)
        {
            const string json = @"{ ""brand_new_field"": ""surprise"" }";

            var result = (IHasJson)Deserialize(type, json);

            result.Internal.Should().NotBeNull();
            result.Internal["brand_new_field"].ToObject<string>().Should().Be("surprise");
        }

        private static object Deserialize(Type type, string json)
        {
            var method = typeof(JsonForCastle)
                .GetMethod(nameof(JsonForCastle.DeserializeObject))
                .MakeGenericMethod(type);

            return method.Invoke(null, new object[] { json });
        }

        private static IDictionary<string, JToken> GetAdditionalData(Type type, object instance)
        {
            return (IDictionary<string, JToken>)type.GetProperty("AdditionalData").GetValue(instance);
        }
    }
}
