using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Messages.Requests;
using Castle.Messages.Responses;

namespace Castle
{
    public class Castle
    {
        private readonly CastleOptions _options;
        private readonly IMessageSender _messageSender;
        private readonly ILogger _logger;

        public Castle(CastleOptions options, ILogger logger = null)
        {
            _options = options;
            _messageSender = options.DoNotTrack 
                ? (IMessageSender) new NoTrackMessageSender()
                : new HttpMessageSender(options);
            _logger = new LoggerWithLevel(logger, options.LogLevel);
        }

        public async Task<Verdict> Authenticate(ActionRequest request)
        {
            return await TryRequest(() => Actions.Authenticate.Execute(
                req => _messageSender.Post<Verdict>("/v1/authenticate", req), 
                request, 
                _options));
        }

        public async Task Track(ActionRequest request)
        {
            await TryRequest(() => Actions.Track.Execute(
                req => _messageSender.Post<VoidResponse>("/v1/track", req),
                request, 
                _options));
        }

        public async Task<DeviceList> GetDevicesForUser(string userId)
        {
            return await TryRequest(() => _messageSender.Get<DeviceList>($"/v1/users/{userId}/devices"));
        }

        public async Task<Device> GetDevice(string deviceToken)
        {
            return await TryRequest(() => _messageSender.Get<Device>($"/v1/devices/{deviceToken}"));
        }

        public async Task ApproveDevice(string deviceToken)
        {
            await TryRequest(() => _messageSender.Put<VoidResponse>($"/v1/devices/{deviceToken}/approve"));
        }

        public async Task ReportDevice(string deviceToken)
        {
            await TryRequest(() => _messageSender.Put<VoidResponse>($"/v1/devices/{deviceToken}/report"));
        }

        public async Task ImpersonateStart(ImpersonateStartRequest request)
        {
            await TryRequest(() => _messageSender.Post<VoidResponse>("/v1/impersonate", request));
        }

        public async Task ImpersonateEnd(ImpersonateEndRequest request)
        {
            await TryRequest(() => _messageSender.Delete<VoidResponse>("/v1/impersonate", request));
        }

        private async Task<T> TryRequest<T>(Func<Task<T>> request)
            where T : new()
        {
            return await ExceptionGuard.Try(request, _logger);
        }
    }
}
