using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Net.Config;
using Castle.Net.Infrastructure.Exceptions;
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
                BaseAddress = options.BaseUrl, 
                Timeout = TimeSpan.FromMilliseconds(options.Timeout)
            };

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + options.ApiSecret));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task Post(ActionRequest payload, string endpoint)
        {
            var jsonContent = PayloadToJson(payload);

            await SendRequest(() => _httpClient.PostAsync(endpoint, jsonContent));
        }

        private static async Task<HttpResponseMessage> SendRequest(Func<Task<HttpResponseMessage>> send)
        {
            try
            {
                var response = await send();
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                throw new CastleInternalException(response);
            }
            catch (TaskCanceledException ce)
            {
                // timeout
                throw new CastleInternalException(ce);
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

    internal interface IMessageSender
    {
        Task Post(ActionRequest payload, string endpoint);
    }
}
