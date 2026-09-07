#if NET48
using System.Collections.Specialized;
using System.Web;
using Castle;
using Castle.Config;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests.Messages
{
    public class When_creating_request_context_for_NetFramework
    {
        [Theory, AutoFakeData]
        public void Should_build_context_from_system_web_request(CastleConfiguration cfg)
        {
            CastleConfiguration.SetConfiguration(cfg);

            var headers = new NameValueCollection
            {
                ["X-Forwarded-For"] = "1.2.3.4"
            };

            var cookies = new HttpCookieCollection();

            var request = Substitute.For<HttpRequestBase>();
            request.Headers.Returns(headers);
            request.Cookies.Returns(cookies);
            request.UserHostAddress.Returns("9.9.9.9");

            var result = Context.FromHttpRequest(request);

            result.Should().NotBeNull();
            result.Ip.Should().Be("1.2.3.4");
            result.Headers.Should().ContainKey("X-Forwarded-For");
        }
    }
}
#endif
