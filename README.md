# .NET SDK for Castle

*TODO: nuget badge*

Supporting .NET Standard 2.0

**[Castle](https://castle.io) analyzes device, location, and interaction patterns in your web and mobile apps and lets you stop account takeover attacks in real-time.**

## Documentation

[Official Castle docs](https://castle.io/docs)

## Installation

Install the `Castle.Net` nuget.

### Command line

    nuget install Castle.Net

### Packet Manager Console

    install-package Castle.Net

### Visual Studio

1. Go to Tools -> Package Manager -> Manage NuGet Packages for Solution...
2. Click the Browse tab and search for `Castle.Net`
3. Click the `Castle.Net` package in the search results, select version and what projects to apply it to on the right side, and click Install

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
Whitelist | | List of headers that should be passed intact to the API. A list of recommended .headers can be retrieved from the static property `Castle.Headers.Whitelist` in the SDK
Blacklist | "Cookie" | List of header that should *not* be passed intact to the API.
DoNotTrack | false | If true, no requests are actually sent to the Caste Api, and Authenticate returns a failover response.

## Logging
The SDK allows customized logging by way of implementing the `ICastleLogger` and passing it to the constructor when creating a `CastleClient` instance. Exactly what gets logged can be controlled by setting the `LogLevel` property of `CastleConfiguration`.

```csharp
var client = new CastleClient(new CastleConfiguration("secret"), new MyLogger());
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

## Actions
The `CastleClient` instance has the action methods **Track** and **Authenticate**. In order to provide the information required for both these methods, you'll need access to the logged in user information (if that is available at that stage in the user flow), as well as request information. In **asp&#46;net core**, both Razor Pages pages and MVC controllers expose the information you need in `User` and `Request` members.

### Track

For events where you don't require a response.

```csharp
await castleClient.Track(new ActionRequest()
{
    Event = Castle.Events.LogoutSucceeded,
    UserId = user.Id,
    UserTraits = new Dictionary<string, string>()
    {
        ["email"] = user.Email,
        ["registered_at"] = user.RegisteredAt
    },
    Context = new RequestContext()
    {
        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        ClientId = Request.Cookies["__cid"],
        Headers = Request.Headers.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault());
    }
});
```

### Authenticate

For events where you require a response. It is used in the same way as `Track`, except that you have the option of waiting for a response.

```csharp
var verdict = await castleClient.Authenticate(new ActionRequest()
{
    Event = Castle.Events.LogoutSucceeded,
    UserId = user.Id,
    UserTraits = new Dictionary<string, string>()
    {
        ["email"] = user.Email,
        ["registered_at"] = user.RegisteredAt
    },
    Context = new RequestContext()
    {
        Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        ClientId = Request.Cookies["__cid"],
        Headers = Request.Headers.ToDictionary(x => x.Key, y => y.Value.FirstOrDefault());
    }
});
```

#### Response format

| Property   | Description                                                                    
| --- | ---
| Action | The recommended action for the given event.
| UserId | The `UserId` of the end user that was provided in the request.
| DeviceToken | The Castle token for the device that generated the event.
| Failover | A boolean indicating if the request failed and the response is a failover.
| FailoverReason | A message indicating why the request failed in case of failover.

#### Failover

When a request to the `/v1/authenticate` endpoint of the Castle API fails, the SDK will generate a failover response based on the `FailoverStrategy` set in the `CastleConfiguration` object.

If no failover strategy is set (i.e. `None`), a `Castle.Infrastructure.Exceptions.CastleExternalException` will be thrown.

### All request options for `Track` and `Authenticate`

| Option | Description
| --- | ---
| Event | The event generated by the user. It can be either an event the SDK constants in `Castle.Events` or a custom one.
| UserId | Your internal ID for the end user.
| UserTraits | An optional, recommended, dictionary of user information, such as `email` and `registered_at`.
| properties | An optional dictionary of custom information.
| Timestamp | An optional datetime indicating when the event occurred, in cases where this might be different from the time when the request is made.
| DeviceToken | The optional device token, used for mitigating or escalating.
| Context | The request context information. See information below.

#### Request context

| Option | Description
| --- | ---
| Ip | The IP address of the request. Note that this needs to be the original request IP, not the IP of an internal proxy, such as nginx.
| ClientId | The client ID, generated by the `c.js` integration on the front end. Commonly found in the `__cid` cookie in `Request.Cookies`, or in some cases the `X-CASTLE-CLIENT-ID` header. 
| Headers | Headers mapped from the the original request (most likely `Request.Headers`).                                                                                       |
## Securing requests

See the documentation on [securing requests](https://castle.io/docs/securing_requests) in order to learn more.

Use the `Castle.Signature.Compute(string key, string message)` method, with your **API Secret** as key, to create a signature to use in the frontend and to validate **Webhooks**.

# Demo application

*TODO: reference/link*
