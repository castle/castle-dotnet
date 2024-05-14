using System.Collections.Generic;
using Castle.Messages.Requests;
using FluentAssertions;
using Xunit;

namespace Tests.Messages
{
    public class When_creating_message_with_context
    {
        [Theory, MemberData(nameof(TestCases))]
        public void Should_set_library_info(RequestContext context)
        {
            context.Library.Name.Should().Be("castle-dotnet");
            context.Library.Version.Split(".").Length.Should().Be(3);
            context.Library.Platform.Should().Be(".NET");

            var platformVersionNumbers = context.Library.PlatformVersion.Replace(".", "");
            int.TryParse(platformVersionNumbers, out _).Should().BeTrue();
        }

        public static IEnumerable<object[]> TestCases => new List<object[]>()
        {
            new object[] { new ActionRequest().Context },
            new object[] { new ImpersonateStartRequest().Context },
            new object[] { new ImpersonateEndRequest().Context },
        };
    }
}
