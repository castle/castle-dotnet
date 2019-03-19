using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.SetUp
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Exception _throws;
        private readonly HttpStatusCode _statusCode = HttpStatusCode.OK;

        public FakeHttpMessageHandler()
        {

        }

        public FakeHttpMessageHandler(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }

        public FakeHttpMessageHandler(Exception throws)
        {
            _throws = throws;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_throws != null)
                throw _throws;

            return Task.FromResult(new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent("{\"prop\":\"fake\"}")
            });
        }
    }
}