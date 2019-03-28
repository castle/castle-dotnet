using System.Threading.Tasks;
using Castle.Actions;
using Castle.Config;
using Castle.Messages.Responses;
using FluentAssertions;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_tracking
    {
        [Theory, AutoFakeData]
        public async Task Should_return_response_if_successful(
            CastleConfiguration configuration)
        {
            var response = new VoidResponse();
            Task<VoidResponse> Send() => Task.FromResult(response);

            var result = await Track.Execute(Send, configuration);

            result.Should().Be(response);
        }
    }
}
