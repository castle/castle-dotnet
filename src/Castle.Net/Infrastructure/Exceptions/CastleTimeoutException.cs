using System;

namespace Castle.Net.Infrastructure.Exceptions
{
    internal class CastleTimeoutException : CastleInternalException
    {
        public CastleTimeoutException(string requestUri, int timeout)
            : base($"Timeout of {timeout} ms exceeded.", requestUri)
        {

        }
    }
}
