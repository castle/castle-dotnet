using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Net.Actions;
using Castle.Net.Config;
using Castle.Net.Infrastructure;
using Castle.Net.Messages;
using NSubstitute;
using Xunit;

namespace Tests
{
    public class When_tracking
    {
        [Theory, AutoData]
        public async Task Should_scrub_headers(
            ActionRequest request,
            CastleOptions options)
        {
            var sender = Substitute.For<IMessageSender>();

            await Track.Execute(sender, request, options);

            await sender.Received().Post<VoidResponse>(
                Arg.Is<ActionRequest>(match => match.Context.Headers != request.Context.Headers),
                Arg.Any<string>());
        }
    }
}
