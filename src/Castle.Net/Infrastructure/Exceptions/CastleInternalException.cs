using System;
using System.Net.Http;

namespace Castle.Net.Infrastructure.Exceptions
{
    class CastleInternalException : Exception
    {
        public CastleInternalException(HttpResponseMessage responseMessage)
        {
            var content = responseMessage.Content.ReadAsStringAsync().Result;
        }

        public CastleInternalException(Exception innerException)
        {

        }
    }
}
