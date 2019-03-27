using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Config;
using Castle.Infrastructure.Exceptions;
using Castle.Infrastructure.Extensions;
using Castle.Infrastructure.Json;

namespace Castle.Infrastructure
{
    internal class HttpMessageSender : IMessageSender
    {
        private readonly IInternalLogger _logger;
        private readonly HttpClient _httpClient;

        public HttpMessageSender(
            CastleConfiguration configuration, 
            IInternalLogger logger) 
            : this(configuration, logger, null)
        {
           
        }

        internal HttpMessageSender(            
            CastleConfiguration configuration,
            IInternalLogger logger,
            HttpMessageHandler handler)
        {
            _logger = logger;

            _httpClient = handler != null ? new HttpClient(handler) : new HttpClient();

            _httpClient.BaseAddress = new Uri(configuration.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromMilliseconds(configuration.Timeout);

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + configuration.ApiSecret));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<TResponse> Post<TResponse>(string endpoint, object payload)
            where TResponse : class, new()
        {
            return await SendRequest<TResponse>(HttpMethod.Post, endpoint, payload);
        }

        public async Task<TResponse> Get<TResponse>(string endpoint) 
            where TResponse : class, new()
        {
            return await SendRequest<TResponse>(HttpMethod.Get, endpoint);
        }

        public async Task<TResponse> Put<TResponse>(string endpoint)
            where TResponse : class, new()
        {
            return await SendRequest<TResponse>(HttpMethod.Put, endpoint);
        }

        public async Task<TResponse> Delete<TResponse>(string endpoint, object payload)
            where TResponse : class, new()
        {
            return await SendRequest<TResponse>(HttpMethod.Delete, endpoint, payload);
        }

        private async Task<TResponse> SendRequest<TResponse>(HttpMethod method, string endpoint, object payload = null)
            where TResponse : class, new()
        {
            var jsonContent = payload != null ? payload.ToHttpContent() : null;
            var requestMessage = new HttpRequestMessage(method, endpoint)
            {
                Content = jsonContent
            };

            _logger.Info(() => string.Concat(
                "Request",
                Environment.NewLine,
                requestMessage,
                Environment.NewLine,
                payload != null ? JsonForCastle.SerializeObject(payload) : ""
            ));

            try
            {                
                var response = await _httpClient.SendAsync(requestMessage);

                var content = response.Content != null ? await response.Content.ReadAsStringAsync() : "";

                _logger.Info(() => string.Concat(
                    "Response",
                    Environment.NewLine,
                    response,
                    Environment.NewLine,
                    content
                ));

                if (response.IsSuccessStatusCode)
                {
                    return JsonForCastle.DeserializeObject<TResponse>(content);
                }

                throw await response.ToCastleException(requestMessage.RequestUri.AbsoluteUri);
            }
            catch (OperationCanceledException)
            {
                throw new CastleTimeoutException(
                    requestMessage.RequestUri.AbsoluteUri, 
                    (int)_httpClient.Timeout.TotalMilliseconds);
            }
        }      
    }
}
