using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Tests.SetUp
{
    public class AutoFakeDataAttribute : AutoDataAttribute
    {
        public AutoFakeDataAttribute()
            : base(() => new Fixture().Customize(new AutoNSubstituteCustomization()))
        {
            
        }
    }
}
