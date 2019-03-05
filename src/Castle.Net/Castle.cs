using System;
using System.Threading.Tasks;
using Castle.Net.Actions;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
using Castle.Net.Infrastructure.Exceptions;
using Castle.Net.Messages;

namespace Castle.Net
{
    public class Castle
    {
        private readonly CastleOptions _options;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;

        public Castle(CastleOptions options, ILogger logger = null)
        {
            _options = options;
            _messageSender = new HttpMessageSender(options);
            _logger = new LoggerWithLevel(logger, options.LogLevel);
        }

        public async Task<ActionResponse> Authenticate(ActionRequest request)
        {
            return await Try(() => new Authenticate(_messageSender).Execute(request));
        }

        private async Task<T> Try<T>(Func<Task<T>> call)
            where T : new()
        {
            try
            {
                return await call();
            }
            catch (CastleExternalException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                return await Task.FromResult(new T());
            }
        }
    }
}
