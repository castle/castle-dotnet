using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure.Exceptions;
using Castle.Net.Infrastructure.Extensions;
using Castle.Net.Messages;

namespace Castle.Net.Infrastructure
{
    internal class HttpMessageSender : IMessageSender
    {
        private readonly HttpClient _httpClient;

        public HttpMessageSender(CastleOptions options)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(options.BaseUrl), 
                Timeout = TimeSpan.FromMilliseconds(options.Timeout)
            };

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + options.ApiSecret));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<TResponse> Post<TResponse>(ActionRequest payload, string endpoint)
        {
            var jsonContent = PayloadToJson(payload);
            var message = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = jsonContent
            };

            return await SendRequest<TResponse>(message);
        }

        private async Task<T> SendRequest<T>(HttpRequestMessage requestMessage)
        {
            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvertForCastle.DeserializeObject<T>(content);
                }

                throw await response.ToCastleException(requestMessage.RequestUri.AbsoluteUri);
            }
            catch (OperationCanceledException)
            {
                throw new CastleTimeoutException(requestMessage.RequestUri.AbsoluteUri, _httpClient.Timeout.Milliseconds);
            }
        }

        private static StringContent PayloadToJson(object payload)
        {
            return new StringContent(
                JsonConvertForCastle.SerializeObject(payload), 
                Encoding.UTF8, 
                "application/json");
        }
    }
}
