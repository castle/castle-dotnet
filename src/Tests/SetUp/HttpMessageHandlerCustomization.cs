using System;
using System.Net;
using System.Net.Http;
using AutoFixture;

namespace Tests.SetUp
{
    public class HttpMessageHandlerSuccessCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<HttpMessageHandler>(() => new FakeHttpMessageHandler());
        }        
    }

    public class HttpMessageHandlerFailureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<HttpMessageHandler>(() => new FakeHttpMessageHandler(HttpStatusCode.InternalServerError));
        }
    }

    public class HttpMessageHandlerCancelledCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register<HttpMessageHandler>(() => new FakeHttpMessageHandler(new OperationCanceledException()));
        }
    }
}
