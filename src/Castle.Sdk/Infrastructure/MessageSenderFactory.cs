using Castle.Config;

namespace Castle.Infrastructure
{
    internal static class MessageSenderFactory
    {
        public static IMessageSender Create(CastleConfiguration configuration, IInternalLogger logger)
        {
            return configuration.DoNotTrack
                ? (IMessageSender)new NoTrackMessageSender()
                : new HttpMessageSender(configuration, logger);
        }
    }
}
