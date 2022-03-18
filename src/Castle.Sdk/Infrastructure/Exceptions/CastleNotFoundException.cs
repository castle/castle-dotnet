using System;
using System.Net;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception for route not found
    /// </summary>
    internal class CastleNotFoundException : Exception
    {
        public CastleNotFoundException(string message, string requestUri, HttpStatusCode? httpStatusCode = null)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            RequestUri = requestUri;
        }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public string RequestUri { get; set; }
    }
}
