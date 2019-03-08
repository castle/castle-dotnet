using System;

namespace Castle.Infrastructure.Exceptions
{
    public class CastleExternalException : Exception
    {
        public CastleExternalException(string message)
            : base(message)
        {
            
        }
    }
}
