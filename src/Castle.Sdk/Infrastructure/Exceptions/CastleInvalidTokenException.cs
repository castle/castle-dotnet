using System;
using System.Net;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    ///  Exception for Invalid request token
    /// </summary>
    public class CastleInvalidTokenException : Exception
    {
        public CastleInvalidTokenException(string message, string requestUri, HttpStatusCode? httpStatusCode = null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
        }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public string RequestUri { get; set; }
    }
}
