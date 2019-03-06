using System;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
using Castle.Net.Infrastructure.Exceptions;
using Castle.Net.Messages;

namespace Castle.Net.Actions
{
    internal static class Authenticate
    {
        public static async Task<AuthenticateResponse> Execute(
            IMessageSender sender,
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

                return await sender.Post<AuthenticateResponse>(alteredRequest, "/v1/authenticate");
            }
            catch (Exception e)
            {
                return CreateFailoverResponse(options.FailOverStrategy, e);
            }
        }

        private static AuthenticateResponse CreateFailoverResponse(ActionType strategy, Exception exception)
        {
            if (strategy == ActionType.None)
            {
                throw new CastleExternalException("Attempted failover, but no strategy was set.");
            }

            return new AuthenticateResponse()
            {
                Action = strategy,
                Failover = true,
                FailoverReason = exception is CastleTimeoutException ? "timeout" : "server error"
            };
        }
    }
}
