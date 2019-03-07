using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.SdkRequests;

namespace Castle.Actions
{
    internal static class Authenticate
    {
        public static async Task<Verdict> Execute(
            Func<ActionRequest, Task<Verdict>> send,
            ActionRequest request,
            CastleOptions options)
        {
            try
            {
                var alteredRequest = request.ShallowCopy();
                if (request.Context != null)
                {
                    var scrubbed = HeaderScrubber.Scrub(request.Context.Headers, options.Whitelist, options.Blacklist);
                    alteredRequest.Context = request.Context.WithHeaders(scrubbed);
                }

                return await send(alteredRequest);
            }
            catch (Exception e)
            {
                return CreateFailoverResponse(options.FailOverStrategy, e);
            }
        }

        private static Verdict CreateFailoverResponse(ActionType strategy, Exception exception)
        {
            if (strategy == ActionType.None)
            {
                throw new CastleExternalException("Attempted failover, but no strategy was set.");
            }

            return new Verdict()
            {
                Action = strategy,
                Failover = true,
                FailoverReason = exception is CastleTimeoutException ? "timeout" : "server error"
            };
        }
    }
}
