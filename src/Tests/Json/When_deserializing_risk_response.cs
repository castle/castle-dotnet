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
    }
}
