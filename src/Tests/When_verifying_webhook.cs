using System;
using System.Security.Cryptography;
using System.Text;
using Castle;
using Castle.Infrastructure.Exceptions;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_verifying_webhook
    {
        private const string ApiSecret = "test_secret";

        private static string Sign(string body)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(ApiSecret)))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(body)));
            }
        }

        [Fact]
        public void Should_not_throw_for_matching_signature()
        {
            const string body = "{\"type\":\"$review.opened\"}";

            Action act = () => Webhook.Verify(body, Sign(body), ApiSecret);

            act.Should().NotThrow();
        }

        [Fact]
        public void Should_throw_for_non_matching_signature()
        {
            Action act = () => Webhook.Verify("{\"a\":1}", "not-the-signature", ApiSecret);

            act.Should().Throw<CastleWebhookVerificationException>();
        }

        [Fact]
        public void Should_throw_for_missing_signature()
        {
            Action act = () => Webhook.Verify("{\"a\":1}", null, ApiSecret);

            act.Should().Throw<CastleWebhookVerificationException>();
        }

        [Fact]
        public void Should_throw_when_api_secret_is_missing()
        {
            Action act = () => Webhook.Verify("{\"a\":1}", "sig", "");

            act.Should().Throw<ArgumentException>();
        }
    }
}
