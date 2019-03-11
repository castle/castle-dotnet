using Castle.Config;

namespace Castle.Infrastructure
{
    internal class LoggerWithLevel : ILogger
    {
        private readonly ILogger _externalLogger;
        private readonly LogLevel _logLevel;

        public LoggerWithLevel(ILogger externalLogger, LogLevel logLevel)
        {
            _externalLogger = externalLogger ?? new NoOpLogger();
            _logLevel = logLevel;
        }

        public void Info(string message)
        {
            WithLevelGuard(() => _externalLogger.Info(message), LogLevel.Info);
        }

        public void Warn(string message)
        {
            WithLevelGuard(() => _externalLogger.Warn(message), LogLevel.Warn);
        }

        public void Error(string message)
        {
            WithLevelGuard(() => _externalLogger.Error(message), LogLevel.Error);
        }

        private void WithLevelGuard(System.Action log, LogLevel level)
        {
            if (_logLevel >= level)
            {
                log();
            }
        }

        private class NoOpLogger : ILogger
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
