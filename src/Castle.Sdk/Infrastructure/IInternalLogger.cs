using System;

namespace Castle.Infrastructure
{
    internal interface IInternalLogger
    {
        void Info(Func<string> createMessage);
        void Warn(Func<string> createMessage);
        void Error(Func<string> createMessage);
    }
}
