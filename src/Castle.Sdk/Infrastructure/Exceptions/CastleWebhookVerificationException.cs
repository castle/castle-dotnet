using System;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    /// Thrown when an incoming webhook cannot be verified against the
    /// <c>X-Castle-Signature</c> header.
    /// </summary>
    public class CastleWebhookVerificationException : Exception
    {
        public CastleWebhookVerificationException(string message)
            : base(message)
        {
        }
    }
}
