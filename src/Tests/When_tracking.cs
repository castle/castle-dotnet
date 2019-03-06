using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Actions;
using Castle.Config;
using Castle.Messages;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class When_tracking
    {
        [Theory, AutoData]
        public async Task Should_scrub_headers(
            ActionRequest request,
            CastleOptions options,
            Verdict response)
        {
            ActionRequest requestArg = null;
            Task<Verdict> Send(ActionRequest req)
            {
                requestArg = req;
                return Task.FromResult(response);
            }

            await Authenticate.Execute(Send, request, options);

            requestArg.Context.Headers.Should().NotEqual(request.Context.Headers);
        }
    }
}
