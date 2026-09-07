using System;
using Castle.Infrastructure.Json;
using Castle.Messages.Responses;
using FluentAssertions;
using Xunit;

namespace Tests.Json
{
    public class When_deserializing_risk_response
    {
        private const string RealApiResponse = @"{
            ""risk"": 0.93,
            ""policy"": {
                ""action"": ""allow"",
                ""id"": ""policy-123"",
                ""name"": ""Default"",
                ""revision_id"": ""rev-456""
            },
            ""scores"": {
                ""account_abuse"": { ""score"": 0.12 },
                ""account_takeover"": { ""score"": 0.05 },
                ""bot"": { ""score"": 0.87 }
            },
            ""signals"": {
                ""new_device"": {},
                ""datacenter_ip"": {}
            },
            ""device"": {
                ""token"": ""dev-token"",
                ""fingerprint"": ""fp-123"",
                ""risk"": 0.5,
                ""created_at"": ""2026-01-01T00:00:00Z"",
                ""last_seen_at"": ""2026-03-01T00:00:00Z""
            }
        }";

        [Fact]
        public void Should_deserialize_scores_from_api_response()
        {
            var result = JsonForCastle.DeserializeObject<RiskResponse>(RealApiResponse);

            result.Scores.Should().NotBeNull();
            result.Scores.Should().ContainKey("bot");
            result.Scores.Should().ContainKey("account_abuse");
            result.Scores.Should().ContainKey("account_takeover");
            result.Scores["bot"].Score.Should().BeApproximately(0.87f, 0.01f);
            result.Scores["account_abuse"].Score.Should().BeApproximately(0.12f, 0.01f);
            result.Scores["account_takeover"].Score.Should().BeApproximately(0.05f, 0.01f);
        }

        [Fact]
        public void Should_deserialize_risk_and_policy()
        {
            var result = JsonForCastle.DeserializeObject<RiskResponse>(RealApiResponse);

            result.Risk.Should().BeApproximately(0.93f, 0.01f);
            result.Policy.Should().NotBeNull();
            result.Policy.Name.Should().Be("Default");
        }

        [Fact]
        public void Should_capture_unknown_fields_in_additional_data()
        {
            const string responseWithNewField = @"{
                ""risk"": 0.42,
                ""brand_new_field"": ""surprise"",
                ""another_new_object"": { ""nested"": 1 }
            }";

            var result = JsonForCastle.DeserializeObject<RiskResponse>(responseWithNewField);

            result.AdditionalData.Should().ContainKey("brand_new_field");
            result.AdditionalData["brand_new_field"].ToObject<string>().Should().Be("surprise");
            result.AdditionalData.Should().ContainKey("another_new_object");
            result.AdditionalData["another_new_object"]["nested"].ToObject<int>().Should().Be(1);
        }

        [Fact]
        public void Should_expose_full_raw_payload_via_internal()
        {
            var result = JsonForCastle.DeserializeObject<RiskResponse>(RealApiResponse);

            result.Internal.Should().NotBeNull();
            result.Internal["risk"].ToObject<float>().Should().BeApproximately(0.93f, 0.01f);
            result.Internal["policy"]["name"].ToObject<string>().Should().Be("Default");
        }

        [Fact]
        public void Should_not_throw_when_fields_are_missing()
        {
            const string sparseResponse = @"{ ""risk"": 0.1 }";

            var result = JsonForCastle.DeserializeObject<RiskResponse>(sparseResponse);

            result.Risk.Should().BeApproximately(0.1f, 0.01f);
            result.Policy.Should().BeNull();
            result.Device.Should().BeNull();
            result.Scores.Should().BeNull();
            result.Failover.Should().BeFalse();
        }

        [Fact]
        public void Should_deserialize_expanded_event_fields()
        {
            const string expanded = @"{
                ""risk"": 0.93,
                ""id"": ""evt-1"",
                ""name"": ""Login Succeeded"",
                ""type"": ""$login"",
                ""status"": ""$succeeded"",
                ""created_at"": ""2026-01-01T00:00:00Z"",
                ""authenticated"": true,
                ""endpoint"": ""/v1/risk"",
                ""email"": { ""value"": ""user@example.com"" },
                ""ip"": { ""address"": ""8.8.8.8"" },
                ""user"": { ""id"": ""u-1"" },
                ""metrics"": { ""users_per_device_fingerprint"": { ""value"": 1 } }
            }";

            var result = JsonForCastle.DeserializeObject<RiskResponse>(expanded);

            result.Id.Should().Be("evt-1");
            result.Name.Should().Be("Login Succeeded");
            result.Type.Should().Be("$login");
            result.Status.Should().Be("$succeeded");
            result.CreatedAt.Should().Be(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            result.Authenticated.Should().BeTrue();
            result.Endpoint.Should().Be("/v1/risk");
            result.Email["value"].ToObject<string>().Should().Be("user@example.com");
            result.Ip["address"].ToObject<string>().Should().Be("8.8.8.8");
            result.User["id"].ToObject<string>().Should().Be("u-1");
            result.Metrics["users_per_device_fingerprint"]["value"].ToObject<int>().Should().Be(1);
        }
    }
}