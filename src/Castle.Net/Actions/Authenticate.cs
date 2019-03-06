using System;
using System.Threading.Tasks;
using Castle.Net.Infrastructure;
using Castle.Net.Infrastructure.Exceptions;
using Castle.Net.Messages;

namespace Castle.Net.Actions
{
    internal static class Authenticate
    {
        public static async Task<AuthenticateResponse> Execute(
            IMessageSender sender,
            AuthenticateRequest request,
            ActionType failoverStrategy)
        {
            try
            {
                return await sender.Post<AuthenticateResponse>(request, "/v1/authenticate");
            }
            catch (Exception e)
            {
                return CreateFailoverResponse(failoverStrategy, e);
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
