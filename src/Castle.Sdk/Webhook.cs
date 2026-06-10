using System;
using System.Security.Cryptography;
using System.Text;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;

namespace Castle
{
    /// <summary>
    /// Verifies the authenticity of incoming Castle webhooks.
    /// </summary>
    public static class Webhook
    {
        /// <summary>
        /// HTTP header carrying the webhook signature.
        /// </summary>
        public const string SignatureHeader = "X-Castle-Signature";

        /// <summary>
        /// Verifies a webhook using the API secret from the active configuration.
        /// </summary>
        /// <param name="requestBody">The raw request body, exactly as received.</param>
        /// <param name="signature">The value of the <c>X-Castle-Signature</c> header.</param>
        /// <exception cref="CastleWebhookVerificationException">Thrown when the signature does not match.</exception>
        public static void Verify(string requestBody, string signature)
        {
            Verify(requestBody, signature, CastleConfiguration.Configuration?.ApiSecret);
        }

        /// <summary>
        /// Verifies a webhook against an explicit API secret.
        /// </summary>
        /// <param name="requestBody">The raw request body, exactly as received.</param>
        /// <param name="signature">The value of the <c>X-Castle-Signature</c> header.</param>
        /// <param name="apiSecret">The Castle API secret.</param>
        /// <exception cref="CastleWebhookVerificationException">Thrown when the signature does not match.</exception>
        public static void Verify(string requestBody, string signature, string apiSecret)
        {
            ArgumentGuard.NotNullOrEmpty(apiSecret, nameof(apiSecret));

            var expected = ComputeSignature(apiSecret, requestBody ?? "");

            if (string.IsNullOrEmpty(signature) || !FixedTimeEquals(expected, signature))
            {
                throw new CastleWebhookVerificationException(
                    "Signature not matching the expected signature");
            }
        }

        private static string ComputeSignature(string key, string message)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(hash);
            }
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);

            if (aBytes.Length != bBytes.Length)
            {
                return false;
            }

            var diff = 0;
            for (var i = 0; i < aBytes.Length; i++)
            {
                diff |= aBytes[i] ^ bBytes[i];
            }

            return diff == 0;
        }
    }
}
