using System;
using Castle.Config;

namespace Castle.Infrastructure
{
    internal class LoggerWithLevels : IInternalLogger
    {
        private readonly ICastleLogger _externalLogger;
        private readonly LogLevel _logLevel;

        public LoggerWithLevels(ICastleLogger externalLogger, LogLevel logLevel)
        {
            _externalLogger = externalLogger ?? new NoOpLogger();
            _logLevel = logLevel;
        }

        // All log methods take a create-func, so we can skip any logging costs in case we don't have to log

        public void Info(Func<string> createMessage)
        {
            WithLevelGuard(() => _externalLogger.Info(createMessage()), LogLevel.Info);
        }

        public void Warn(Func<string> createMessage)
        {
            WithLevelGuard(() => _externalLogger.Warn(createMessage()), LogLevel.Warn);
        }

        public void Error(Func<string> createMessage)
        {
            WithLevelGuard(() => _externalLogger.Error(createMessage()), LogLevel.Error);
        }

        private void WithLevelGuard(Action log, LogLevel level)
        {
            if (_logLevel >= level)
            {
                log();
            }
        }

        private class NoOpLogger : ICastleLogger
        {
            public void Info(string message)
            {
                
            }

            public void Warn(string message)
            {
                
            }

            public void Error(string message)
            {
                
            }
        }        
    }
}
