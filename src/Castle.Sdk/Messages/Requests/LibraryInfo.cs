using System.Reflection;
using System.Runtime.InteropServices;

namespace Castle.Messages.Requests
{
    public class LibraryInfo
    {
        public string Name { get; } = "castle-dotnet";

        public string Version { get; } = 
            new AssemblyName(typeof(LibraryInfo).GetTypeInfo().Assembly.FullName)
                .Version
                .ToString(3);

        public string Platform { get; } = GetPlatformName();

        public string PlatformVersion { get; } = GetPlatformVersion();

        private static string GetPlatformName()
        {
            var description = RuntimeInformation.FrameworkDescription;
            var lastSpace = description.LastIndexOf(' ');
            return lastSpace > 0 ? description.Substring(0, lastSpace) : description;
        }

        private static string GetPlatformVersion()
        {
            var description = RuntimeInformation.FrameworkDescription;
            var lastSpace = description.LastIndexOf(' ');
            return lastSpace > 0 && lastSpace < description.Length - 1
                ? description.Substring(lastSpace + 1)
                : string.Empty;
        }
    }
}
