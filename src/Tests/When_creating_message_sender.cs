using Castle.Config;
using Castle.Infrastructure;
using FluentAssertions;
using NSubstitute;
using Tests.SetUp;
using Xunit;

namespace Tests
{
    public class When_creating_message_sender
    {
        [Theory, AutoFakeData]
        public void Should_create_real_sender_if_do_not_track_is_disabled(CastleConfiguration configuration)
        {
            var logger = Substitute.For<IInternalLogger>();
            configuration.DoNotTrack = false;
            var result = MessageSenderFactory.Create(configuration, logger);

            result.Should().BeOfType<HttpMessageSender>();
        }

        [Theory, AutoFakeData]
        public void Should_create_fake_sender_if_do_not_track_is_enabled(CastleConfiguration configuration)
        {
            var logger = Substitute.For<IInternalLogger>();
            configuration.DoNotTrack = true;
            var result = MessageSenderFactory.Create(configuration, logger);

            result.Should().BeOfType<NoTrackMessageSender>();
        }
    }
}
