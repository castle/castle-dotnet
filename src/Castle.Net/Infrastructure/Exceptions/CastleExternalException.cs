using System;

namespace Castle.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception that will be allowed to escape the SDK
    /// </summary>
    public class CastleExternalException : Exception
    {
        public CastleExternalException(string message)
            : base(message)
        {
            
        }
    }
}
