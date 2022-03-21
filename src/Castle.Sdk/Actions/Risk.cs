using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Exceptions;
using Castle.Messages;
using Castle.Messages.Responses;

namespace Castle.Actions
{
    internal static class Risk
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
            catch (Exception e) when (e is CastleClientErrorException || e is CastleInvalidTokenException || e is CastleInvalidParametersException)
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
                Policy = new Policy
                {
                    Action = strategy
                },
                Failover = true,
                FailoverReason = reason
            };
        }

        private static RiskResponse CreateFailoverResponse(ActionType strategy, Exception exception)
        {
            return CreateFailoverResponse(strategy, exception is CastleTimeoutException ? "timeout" : "server error");
        }
    }
}
