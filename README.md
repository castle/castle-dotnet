# .NET SDK for Castle
[![Build status](https://ci.appveyor.com/api/projects/status/rf0304hhym6k7d7s/branch/master?svg=true)](https://ci.appveyor.com/project/DevTools/castle-dotnet)
[![NuGet](https://img.shields.io/nuget/v/castle.sdk.svg)](https://www.nuget.org/packages/Castle.Sdk/)
[![Coverage Status](https://coveralls.io/repos/github/castle/castle-dotnet/badge.svg?branch=master)](https://coveralls.io/github/castle/castle-dotnet?branch=master)

Supporting **.NET Standard 2.0** and **.NET Framework 4.6.1+**. Refer to Microsoft's [documentation](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) for compatibility information.

**[Castle](https://castle.io) analyzes user behavior in web and mobile apps to stop fraud before it happens.**

## Usage

See the [documentation](https://docs.castle.io) for how to use this SDK with the Castle APIs

## Installation

Install the `Castle.Sdk` nuget.

### Command line

    nuget install Castle.Sdk

### Packet Manager Console

    install-package Castle.Sdk

### Visual Studio

1. Go to Tools -> Package Manager -> Manage NuGet Packages for Solution...
2. Click the Browse tab and search for `Castle.Sdk`
3. Click the `Castle.Sdk` package in the search results, select version and what projects to apply it to on the right side, and click Install

## Configuration

Go to the settings page of your Castle account and find your **API Secret**. Use it to create a new instance of the `CastleClient` class.

```csharp
var client = new CastleClient("YOUR SECRET");
```

It's a good idea to set up your `CastleClient` instance using an IoC container.

### asp&#46;net core
```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...

    services.AddSingleton(new CastleClient(new CastleConfiguration("YOUR SECRET")));
}
```

The `CastleConfiguration` object has a number of properties that control the SDK.

Property | Default | Description
--- | --- | ---
ApiSecret | | Secret used to authenticate with the Castle Api. ***Required***
FailoverStrategy | Allow | The response action to return in case of a failover in an Authenticate request.
Timeout | 1000 | Timeout for requests, in milliseconds.
BaseUrl | https://api.castle.io | Base Castle Api url.
LogLevel | Error | The log level applied by the injected `ICastleLogger` implementation.
AllowList | | List of headers that should be passed intact to the API. A list of recommended .headers can be retrieved from the static property `Castle.Headers.AllowList` in the SDK
DenyList | | List of header that should *not* be passed intact to the API.
DoNotTrack | false | If true, no requests are actually sent to the Caste Api, and Authenticate returns a failover response.
Logger | | Your own logger implementation.
IpHeaders | | IP Headers to look for a client IP address
TrustedProxies | | Trusted public proxies list
TrustedProxyDepth | 0 | Number of trusted proxies used in the chain
TrustProxyChain | false | Is trusting all of the proxy IPs in X-Forwarded-For enabled

## Logging
The SDK allows customized logging by way of implementing the `ICastleLogger` interface and passing in an instance as part of the  `CastleConfiguration`. Exactly what gets logged can be controlled by setting the `LogLevel` property of `CastleConfiguration`.

```csharp
var client = new CastleClient(new CastleConfiguration("secret") {
    Logger = new MyLogger()
});
```

### Logger example
```csharp
public class DebugLogger : ICastleLogger
{
    public void Info(string message)
    {
        Debug.WriteLine($"INFO: {message}");
    }

    public void Warn(string message)
    {
        Debug.WriteLine($"WARNING: {message}");
    }

    public void Error(string message)
    {
        Debug.WriteLine($"ERROR: {message}");
    }
}
```

##### ASP&#46;NET MVC 5
```csharp
public class HomeController : Controller
{
    public ActionResult Index()
    {
        var actionRequest = new ActionRequest()
        {
            Context = Castle.Context.FromHttpRequest(Request)
            ...
```

##### ASP&#46;NET Core
```csharp
public class IndexModel : PageModel
{
    public void OnGet()
    {
        var actionRequest = new ActionRequest()
        {
            Context = Castle.Context.FromHttpRequest(Request)
            ...
```


## Troubleshooting
### Can't find System.Runtime.InteropServices.RuntimeInformation
You target .NET Framework and get an exception on startup.

`System.IO.FileNotFoundException: 'Could not load file or assembly 'System.Runtime.InteropServices.RuntimeInformation, Version=4.0.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies. The system cannot find the file specified.`

The Castle SDK has a dependency on [Sentry.PlatformAbstractions](https://www.nuget.org/packages/Sentry.PlatformAbstractions/), which in turn uses `System.Runtime.InteropServices.RuntimeInformation`, version 4.3.0.

#### Solution
Find the binding redirect for `System.Runtime.InteropServices.RuntimeInformation` in `web.config` and either remove the entire `dependentAssembly` element, or update `newVersion` to 4.3.0.


# Demo application

There is a sample application using ASP&#46;NET Core Razor Pages and this SDK [here](https://github.com/castle/dotnet-example).
