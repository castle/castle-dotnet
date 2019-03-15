using System.Reflection;

namespace Castle.Messages.Requests
{
    public class LibraryInfo
    {
        public string Name { get; } = "castle-dotnet";

        public string Version { get; } = 
            new AssemblyName(typeof(LibraryInfo).GetTypeInfo().Assembly.FullName)
                .Version
                .ToString(3);

        public string Platform { get; } = Sentry.PlatformAbstractions.Runtime.Current.Name;

        public string PlatformVersion { get; } = Sentry.PlatformAbstractions.Runtime.Current.Version;
    }
}