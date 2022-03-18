using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Filter
    {
        public static async Task<RiskResponse> Execute(
            Func<Task<RiskResponse>> send,
            CastleConfiguration configuration,
            IInternalLogger logger)
        {
            try
            {
                return await send();
            }
            catch (CastleNotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                logger.Warn(() => "Failover, " + e);
                return CreateFailoverResponse(configuration.FailOverStrategy, e);
            }
        }

        private static RiskResponse CreateFailoverResponse(ActionType strategy, string reason)
        {
            if (strategy == ActionType.None)
            {
                throw new CastleExternalException("Attempted failover, but no strategy was set.");
            }

            return new RiskResponse()
            {
                Action = strategy,
                Failover = true,
                FailoverReason = reason
            };
        }

        private static RiskResponse CreateFailoverResponse(ActionType strategy, Exception exception)
        {
            if (exception is CastleInvalidTokenException)
            {
                return CreateFailoverResponse(ActionType.Deny, exception.Message);
            }
            if (exception is CastleInvalidParametersException)
            {
                return CreateFailoverResponse(strategy, exception.Message);
            }
            return CreateFailoverResponse(strategy, exception is CastleTimeoutException ? "timeout" : "server error");
        }
    }
}
