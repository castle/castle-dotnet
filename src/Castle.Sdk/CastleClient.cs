using System;
using System.Collections.Generic;
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


        #region lists

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<ListResponse> CreateList(CreateListRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Post<ListResponse>("/v1/lists", request));
        }

        public async Task<List<ListResponse>> GetAllLists()
        {
            return await TryRequest(() => _messageSender.Get<List<ListResponse>>("/v1/lists"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        public async Task<ListResponse> GetList(string listId)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));

            return await TryRequest(() => _messageSender.Get<ListResponse>($"/v1/lists/{listId}"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<ListResponse> UpdateList(string listId, UpdateListRequest request)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Put<ListResponse>($"/v1/lists/{listId}", request));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        public async Task DeleteList(string listId)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));

            await TryRequest(() => _messageSender.Delete<VoidResponse>($"/v1/lists/{listId}", null));
        }

        public async Task<List<ListResponse>> QueryLists(SearchQuery query)
        {
            return await TryRequest(() => _messageSender.Post<List<ListResponse>>("/v1/lists/query", query ?? new SearchQuery()));
        }

        #endregion

        #region list items

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<ListItemResponse> CreateListItem(string listId, CreateListItemRequest request)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Post<ListItemResponse>($"/v1/lists/{listId}/items", request));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> or <paramref name="itemId"/> is null or empty</exception>
        public async Task<ListItemResponse> GetListItem(string listId, string itemId)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNullOrEmpty(itemId, nameof(itemId));

            return await TryRequest(() => _messageSender.Get<ListItemResponse>($"/v1/lists/{listId}/items/{itemId}"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> or <paramref name="itemId"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<ListItemResponse> UpdateListItem(string listId, string itemId, UpdateListItemRequest request)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNullOrEmpty(itemId, nameof(itemId));
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Put<ListItemResponse>($"/v1/lists/{listId}/items/{itemId}", request));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        public async Task<List<ListItemResponse>> QueryListItems(string listId, SearchQuery query)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));

            return await TryRequest(() => _messageSender.Post<List<ListItemResponse>>($"/v1/lists/{listId}/items/query", query ?? new SearchQuery()));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        public async Task<CountResponse> CountListItems(string listId, CountListItemsRequest request)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));

            return await TryRequest(() => _messageSender.Post<CountResponse>($"/v1/lists/{listId}/items/count", request ?? new CountListItemsRequest()));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> or <paramref name="itemId"/> is null or empty</exception>
        public async Task ArchiveListItem(string listId, string itemId)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNullOrEmpty(itemId, nameof(itemId));

            await TryRequest(() => _messageSender.Delete<VoidResponse>($"/v1/lists/{listId}/items/{itemId}/archive", null));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> or <paramref name="itemId"/> is null or empty</exception>
        public async Task UnarchiveListItem(string listId, string itemId)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNullOrEmpty(itemId, nameof(itemId));

            await TryRequest(() => _messageSender.Put<VoidResponse>($"/v1/lists/{listId}/items/{itemId}/unarchive"));
        }

        /// <exception cref="ArgumentException">Thrown when <paramref name="listId"/> is null or empty</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<BatchListItemsResponse> CreateBatchListItems(string listId, BatchListItemsRequest request)
        {
            ArgumentGuard.NotNullOrEmpty(listId, nameof(listId));
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Post<BatchListItemsResponse>($"/v1/lists/{listId}/items/batch", request));
        }

        #endregion

        #region privacy

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task RequestUserData(PrivacyRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            await TryRequest(() => _messageSender.Post<VoidResponse>("/v1/privacy/users", request));
        }

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task DeleteUserData(PrivacyRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            await TryRequest(() => _messageSender.Delete<VoidResponse>("/v1/privacy/users", request));
        }

        #endregion

        #region events

        public async Task<EventsSchemaResponse> EventsSchema()
        {
            return await TryRequest(() => _messageSender.Get<EventsSchemaResponse>("/v1/events/schema"));
        }

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<EventsResponse> QueryEvents(EventsQueryRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Post<EventsResponse>("/v1/events/query", request));
        }

        /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null</exception>
        public async Task<EventsResponse> GroupEvents(EventsGroupRequest request)
        {
            ArgumentGuard.NotNull(request, nameof(request));

            return await TryRequest(() => _messageSender.Post<EventsResponse>("/v1/events/group", request));
        }

        #endregion

        private async Task<T> TryRequest<T>(Func<Task<T>> request)
            where T : class
        {
            return await ExceptionGuard.Try(request, _logger);
        }
    }
}
