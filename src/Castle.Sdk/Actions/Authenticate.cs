using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;
using Newtonsoft.Json.Linq;

namespace Castle.Actions
{
    internal static class Authenticate
    {
        public static async Task<Verdict> Execute(
            Func<JObject, Task<Verdict>> send,
            JObject request,
            CastleConfiguration configuration,
            IInternalLogger logger)
        {
            if (configuration.DoNotTrack)
                return CreateFailoverResponse(configuration.FailOverStrategy, "do not track");

            try
            {                
                return await send(request);
            }
            catch (Exception e)
            {
                logger.Warn(() => "Failover, " + e);
                return CreateFailoverResponse(configuration.FailOverStrategy, e);
            }
        }

        private static Verdict CreateFailoverResponse(ActionType strategy, string reason)
        {
            if (strategy == ActionType.None)
            {
                throw new CastleExternalException("Attempted failover, but no strategy was set.");
            }

            return new Verdict()
            {
                Action = strategy,
                Failover = true,
                FailoverReason = reason
            };
        }

        private static Verdict CreateFailoverResponse(ActionType strategy, Exception exception)
        {
            return CreateFailoverResponse(strategy, exception is CastleTimeoutException ? "timeout" : "server error");
        }
    }
}
