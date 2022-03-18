using System;
using System.Threading.Tasks;
using Castle.Infrastructure.Exceptions;

namespace Castle.Infrastructure
{
    internal static class ExceptionGuard
    {
        public static async Task<TResponse> Try<TResponse>(
            Func<Task<TResponse>> request,
            IInternalLogger logger)
            where TResponse : class
        {
            try
            {
                return await request();
            }
            catch (CastleExternalException)
            {
                throw;
            }
            catch (CastleNotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.ToString);
                return null;
            }
        }
    }
}
