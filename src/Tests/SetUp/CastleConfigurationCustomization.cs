using AutoFixture;
using Castle.Config;

namespace Tests.SetUp
{
    public class CastleConfigurationCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CastleConfiguration>(composer => composer.OmitAutoProperties());
        }
    }
}
