using System;
using System.Net;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for Payment Required
    /// </summary>
    public class CastlePaymentRequiredException : Exception
    {
        public CastlePaymentRequiredException(string message, string requestUri, HttpStatusCode? httpStatusCode = null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
        }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public string RequestUri { get; set; }
    }
}
