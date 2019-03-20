using System;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Tests.SetUp
{
    public class AutoFakeDataAttribute : AutoDataAttribute
    {
        public AutoFakeDataAttribute(params Type[] customizationTypes)
            : base(() =>
            {
                var standard = Customizations.Get();
                var fixture = new  Fixture().Customize(standard);

                foreach (var type in customizationTypes)
                {
                    var customization = Activator.CreateInstance(type) as ICustomization;
                    fixture.Customize(customization);
                }

                return fixture;
            })
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
            new CastleConfigurationDefaultCustomization()
        );
    }
}
