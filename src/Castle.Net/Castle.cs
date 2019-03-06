using System;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
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

        public async Task<AuthenticateResponse> Authenticate(ActionRequest request)
        {
            return await TryRequest(() => Actions.Authenticate.Execute(_messageSender, request, _options));
        }

        public async Task Track(ActionRequest request)
        {
            await TryRequest(() => Actions.Track.Execute(_messageSender, request, _options));
        }

        private async Task<T> TryRequest<T>(Func<Task<T>> request)
            where T : new()
        {
            return await ExceptionGuard.Try(request, _logger);
        }
    }
}
