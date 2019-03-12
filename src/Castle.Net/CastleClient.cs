using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Messages.Requests;
using Castle.Messages.Responses;

namespace Castle
{
    /// <summary>
    /// Main SDK entry point
    /// </summary>
    public class CastleClient
    {
        private readonly CastleConfiguration _configuration;
        private readonly IMessageSender _messageSender;
        private readonly IInternalLogger _logger;

        public CastleClient(CastleConfiguration configuration)
        {
            _configuration = configuration;
            
            _logger = new LoggerWithLevels(configuration.Logger, configuration.LogLevel);

            _messageSender = configuration.DoNotTrack
                ? (IMessageSender)new NoTrackMessageSender()
                : new HttpMessageSender(configuration, _logger);
        }

        public async Task<Verdict> Authenticate(ActionRequest request)
        {
            return await TryRequest(() => Actions.Authenticate.Execute(
                req => _messageSender.Post<Verdict>("/v1/authenticate", req), 
                request, 
                _configuration,
                _logger));
        }

        public async Task Track(ActionRequest request)
        {
            await TryRequest(() => Actions.Track.Execute(
                req => _messageSender.Post<VoidResponse>("/v1/track", req),
                request, 
                _configuration));
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
