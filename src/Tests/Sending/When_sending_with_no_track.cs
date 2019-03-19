using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Castle.Infrastructure;
using FluentAssertions;
using Xunit;

namespace Tests.Sending
{
    public class When_sending_with_no_track
    {
        [Theory, AutoData]
        public async Task Should_return_new_instance_from_Get(string endpoint)
        {
            var sut = new NoTrackMessageSender();
            var expected = new GenericObject();

            var result = await sut.Get<GenericObject>(endpoint);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, AutoData]
        public async Task Should_return_new_instance_from_Post(string endpoint, object payload)
        {
            var sut = new NoTrackMessageSender();
            var expected = new GenericObject();

            var result = await sut.Post<GenericObject>(endpoint, payload);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, AutoData]
        public async Task Should_return_new_instance_from_Put(string endpoint)
        {
            var sut = new NoTrackMessageSender();
            var expected = new GenericObject();

            var result = await sut.Put<GenericObject>(endpoint);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory, AutoData]
        public async Task Should_return_new_instance_from_Delete(string endpoint, object payload)
        {
            var sut = new NoTrackMessageSender();
            var expected = new GenericObject();

            var result = await sut.Delete<GenericObject>(endpoint, payload);
            result.Should().BeEquivalentTo(expected);
        }

        private class GenericObject
        {
            public string GenericProperty { get; set; }
        }
    }
}
