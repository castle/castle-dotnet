using System;
using System.Threading.Tasks;
using Castle.Infrastructure.Exceptions;

namespace Castle.Infrastructure
{
    internal static class ExceptionGuard
    {
        public static async Task<TResponse> Try<TResponse>(
            Func<Task<TResponse>> request, 
            ILogger logger)
            where TResponse : new()
        {
            try
            {
                return await request();
            }
            catch (CastleExternalException)
            {
                throw;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                return await Task.FromResult(new TResponse());
            }
        }
    }
}
