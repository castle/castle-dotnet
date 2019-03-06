using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Messages;

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
            _messageSender = new HttpMessageSender(options);
            _logger = new LoggerWithLevel(logger, options.LogLevel);
        }

        public async Task<Verdict> Authenticate(ActionRequest request)
        {
            return await TryRequest(() => Actions.Authenticate.Execute(
                req => _messageSender.Post<Verdict>(req, "/v1/authenticate"), 
                request, 
                _options));
        }

        public async Task Track(ActionRequest request)
        {
            await TryRequest(() => Actions.Track.Execute(
                req => _messageSender.Post<VoidResponse>(req, "/v1/track"),
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

        private async Task<T> TryRequest<T>(Func<Task<T>> request)
            where T : new()
        {
            return await ExceptionGuard.Try(request, _logger);
        }

        /*
        - /v1/devices/:device_token/approve (approveDevice)
        - /v1/devices/:device_token/report (reportDevice)
        - /v1/impersonate (impersonate)
        */
    }
}
