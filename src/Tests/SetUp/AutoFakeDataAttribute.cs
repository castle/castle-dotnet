using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Tests.SetUp
{
    public class AutoFakeDataAttribute : AutoDataAttribute
    {
        public AutoFakeDataAttribute()
            : base(() => new Fixture().Customize(Customizations.Get()))
        {
            
        }        
    }

    public class InlineAutoFakeDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoFakeDataAttribute(params object[] values)
            : base(new AutoFakeDataAttribute(), values)
        {
            
        }
    }

    public static class Customizations
    {
        public static CompositeCustomization Get() => new CompositeCustomization(
            new AutoNSubstituteCustomization(),
            new CastleConfigurationCustomization()
        );
    }
}
