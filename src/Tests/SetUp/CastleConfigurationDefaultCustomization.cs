using AutoFixture;
using Castle.Config;

namespace Tests.SetUp
{
    public class CastleConfigurationDefaultCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CastleConfiguration>(composer => composer
                .OmitAutoProperties());
        }
    }

    public class CastleConfigurationNoTrackCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CastleConfiguration>(composer => composer
                .OmitAutoProperties()
                .With(x => x.DoNotTrack, true));
        }
    }
}
