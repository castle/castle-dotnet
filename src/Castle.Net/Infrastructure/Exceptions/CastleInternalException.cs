using System;
using System.Net;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for internal SDK errors, which must not escape to the outside
    /// </summary>
    internal class CastleInternalException : Exception
    {
        protected CastleInternalException(string message)
            : base(message)
        {

        }

        protected CastleInternalException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public CastleInternalException(string message, string requestUri, HttpStatusCode? httpStatusCode = null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
        }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public string RequestUri { get; set; }
    }
}
