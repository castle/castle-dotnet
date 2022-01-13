using System;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure;
using Castle.Infrastructure.Json;
using Castle.Messages.Requests;
using Castle.Messages.Responses;
using Newtonsoft.Json.Linq;

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

        /// <exception cref="ArgumentException">Thrown when <paramref name="configuration"/> is null</exception>
        public CastleClient(CastleConfiguration configuration)
        {
            ArgumentGuard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
            CastleConfiguration.SetConfiguration(configuration);

            _logger = new LoggerWithLevels(configuration.Logger, configuration.LogLevel);

            _messageSender = MessageSenderFactory.Create(configuration, _logger);
        }

        public JObject BuildAuthenticateRequest(ActionRequest request)
        {
            var prepared = (request ?? new ActionRequest()).PrepareApiCopy(_configuration.AllowList, _configuration.DenyList);
            return JsonForCastle.FromObject(prepared);
        }

        public async Task<Verdict> SendAuthenticateRequest(JObject request)
        {
            return await TryRequest(() => Actions.Authenticate.Execute(
                () => _messageSender.Post<Verdict>("/v1/authenticate", request),
                _configuration,
                _logger));
        }

        public async Task<Verdict> Authenticate(ActionRequest request)
        {
            var jsonRequest = BuildAuthenticateRequest(request);

            return await SendAuthenticateRequest(jsonRequest);
        }

        public JObject BuildTrackRequest(ActionRequest request)
        {
            var prepared = (request ?? new ActionRequest()).PrepareApiCopy(_configuration.AllowList, _configuration.DenyList);
            return JsonForCastle.FromObject(prepared);
        }

        public async Task SendTrackRequest(JObject request)
        {
            await TryRequest(() => Actions.Track.Execute(
                () => _messageSender.Post<VoidResponse>("/v1/track", request),
                _configuration));
        }

        public async Task Track(ActionRequest request)
        {
            var jsonRequest = BuildTrackRequest(request);

            await SendTrackRequest(jsonRequest);
        }

        #region filter

        public JObject BuildFilterRequest(ActionRequest request)
        {
            var prepared = (request ?? new ActionRequest()).PrepareApiCopy(_configuration.AllowList, _configuration.DenyList);
            return JsonForCastle.FromObject(prepared);
        }

        public async Task<RiskResponse> SendFilterRequest(JObject request)
        {
            return await TryRequest(() => Actions.Filter.Execute(
                 () => _messageSender.Post<RiskResponse>("/v1/filter", request),
                 _configuration, _logger));
        }

        public async Task<RiskResponse> Filter(ActionRequest request)
        {
            var jsonRequest = BuildFilterRequest(request);

            return await SendFilterRequest(jsonRequest);
        }
        #endregion


        #region risk
        public async Task<RiskResponse> Risk(ActionRequest request)
        {
            var jsonRequest = BuildRiskRequest(request);

            return await SendRiskRequest(jsonRequest);
        }


        public JObject BuildRiskRequest(ActionRequest request)
        {
            var prepared = (request ?? new ActionRequest()).PrepareApiCopy(_configuration.AllowList, _configuration.DenyList);
            return JsonForCastle.FromObject(prepared);
        }

        public async Task<RiskResponse> SendRiskRequest(JObject request)
        {
            return await TryRequest(() => Actions.Risk.Execute(
                () => _messageSender.Post<RiskResponse>("/v1/risk", request),
                _configuration, _logger));
        }

        #endregion

        #region log

        public JObject BuildLogRequest(ActionRequest request)
        {
            var prepared = (request ?? new ActionRequest()).PrepareApiCopy(_configuration.AllowList, _configuration.DenyList);
            return JsonForCastle.FromObject(prepared);
        }

        public async Task SendLogRequest(JObject request)
        {
            await TryRequest(() => Actions.Log.Execute(
                () => _messageSender.Post<VoidResponse>("/v1/log", request),
                _configuration));
        }

        public async Task Log(ActionRequest request)
        {
            var jsonRequest = BuildLogRequest(request);

            await SendLogRequest(jsonRequest);
        }

        #endregion


        /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty</exception>>
        public async Task<DeviceList> GetDevicesForUser(string userId, string clientId = null)
        {
            ArgumentGuard.NotNullOrEmpty(userId, nameof(userId));

            var endpoint = QueryStringBuilder.Append($"/v1/users/{userId}/devices", "client_id", clientId);
            return await TryRequest(() => _messageSender.Get<DeviceList>(endpoint));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="deviceToken"/> is null or empty</exception>>
        public async Task<Device> GetDevice(string deviceToken)
        {
            ArgumentGuard.NotNullOrEmpty(deviceToken, nameof(deviceToken));

            return await TryRequest(() => _messageSender.Get<Device>($"/v1/devices/{deviceToken}"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="deviceToken"/> is null or empty</exception>>
        public async Task ApproveDevice(string deviceToken)
        {
            ArgumentGuard.NotNullOrEmpty(deviceToken, nameof(deviceToken));

            await TryRequest(() => _messageSender.Put<VoidResponse>($"/v1/devices/{deviceToken}/approve"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="deviceToken"/> is null or empty</exception>>
        public async Task ReportDevice(string deviceToken)
        {
            ArgumentGuard.NotNullOrEmpty(deviceToken, nameof(deviceToken));

            await TryRequest(() => _messageSender.Put<VoidResponse>($"/v1/devices/{deviceToken}/report"));
        }

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>>
        public async Task ImpersonateStart(ImpersonateStartRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            await TryRequest(() => _messageSender.Post<VoidResponse>("/v1/impersonate", request));
        }

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>>
        public async Task ImpersonateEnd(ImpersonateEndRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            await TryRequest(() => _messageSender.Delete<VoidResponse>("/v1/impersonate", request));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="userId"/> is null or empty</exception>>
        public async Task<User> ArchiveDevices(string userId)
        {
            ArgumentGuard.NotNullOrEmpty(userId, nameof(userId));

            return await TryRequest(() => _messageSender.Put<User>($"/v1/users/{userId}/archive_devices"));
        }

        private async Task<T> TryRequest<T>(Func<Task<T>> request)
            where T : class
        {
            return await ExceptionGuard.Try(request, _logger);
        }
    }
}
