using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Requests;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Authenticate
    {
        public static async Task<Verdict> Execute(
            Func<ActionRequest, Task<Verdict>> send,
            ActionRequest request,
            CastleOptions options,
            ILogger logger)
        {
            if (options.DoNotTrack)
                return CreateFailoverResponse(options.FailOverStrategy, "do not track");

            try
            {
                var apiRequest = request.PrepareApiCopy(options.Whitelist, options.Blacklist);

                return await send(apiRequest);
            }
            catch (Exception e)
            {
                logger.Warn("Failover, " + e);
                return CreateFailoverResponse(options.FailOverStrategy, e);
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
