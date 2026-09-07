# .NET SDK for Castle

[![NuGet](https://img.shields.io/nuget/v/castle.sdk.svg)](https://www.nuget.org/packages/Castle.Sdk/)

Supporting **.NET 10.0**, **.NET 8.0**, **.NET Standard 2.0** and **.NET Framework 4.8**. Through .NET Standard 2.0 the SDK is also consumable from .NET Core 2.0+, .NET 5+, and .NET Framework 4.6.1+; refer to Microsoft's [documentation](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) for compatibility information.

**[Castle](https://castle.io) analyzes user behavior in web and mobile apps to stop fraud before it happens.**

## Usage

See the [documentation](https://docs.castle.io) for how to use this SDK with the Castle APIs.

## Installation

Install the `Castle.Sdk` NuGet package.

### .NET CLI

    dotnet add package Castle.Sdk

### Package Manager Console

    Install-Package Castle.Sdk

### Visual Studio

1. Go to Tools -> Package Manager -> Manage NuGet Packages for Solution...
2. Click the Browse tab and search for `Castle.Sdk`
3. Click the `Castle.Sdk` package in the search results, select version and what projects to apply it to on the right side, and click Install

## Configuration

Go to the settings page of your Castle account and find your **API Secret**. Use it to create a new instance of the `CastleClient` class.

```csharp
var client = new CastleClient(new CastleConfiguration("YOUR SECRET"));
```

It's a good idea to set up your `CastleClient` instance using an IoC container.

### ASP&#46;NET Core

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton(new CastleClient(new CastleConfiguration("YOUR SECRET")));
}
```

The `CastleConfiguration` object has a number of properties that control the SDK.

| Property          | Default               | Description                                                                                                                                                             |
| ----------------- | --------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| ApiSecret         |                       | Secret used to authenticate with the Castle Api. **_Required_**                                                                                                         |
| FailOverStrategy  | Allow                 | The response action to return in case of a failover in a Risk or Filter request.                                                                                        |
| Timeout           | 1000                  | Timeout for requests, in milliseconds.                                                                                                                                  |
| BaseUrl           | https://api.castle.io | Base Castle Api url.                                                                                                                                                    |
| LogLevel          | Error                 | The log level applied by the injected `ICastleLogger` implementation.                                                                                                   |
| AllowList         |                       | List of headers that should be passed intact to the API. A list of recommended headers can be retrieved from the static property `Castle.Headers.AllowList` in the SDK. |
| DenyList          |                       | List of headers that should _not_ be passed intact to the API.                                                                                                          |
| DoNotTrack        | false                 | If true, no requests are actually sent to the Castle Api, and Risk/Filter return a failover response.                                                                    |
| Logger            |                       | Your own logger implementation.                                                                                                                                         |
| IpHeaders         |                       | IP Headers to look for a client IP address.                                                                                                                             |
| TrustedProxies    |                       | Trusted public proxies list.                                                                                                                                            |
| TrustedProxyDepth | 0                     | Number of trusted proxies used in the chain.                                                                                                                            |
| TrustProxyChain   | false                 | Is trusting all of the proxy IPs in X-Forwarded-For enabled.                                                                                                            |

## API Actions

All API action methods accept an `ActionRequest` object and are async.

### Risk

```csharp
var response = await client.Risk(new ActionRequest()
{
    Event = "$login",
    Status = "$succeeded",
    UserId = "user-123",
    RequestToken = "token-from-castle-js",
    Context = Castle.Context.FromHttpRequest(Request)
});

// response.Risk       - risk score (float)
// response.Policy     - policy evaluation result
// response.Signals    - signal details
// response.Device     - device information
```

### Filter

```csharp
var response = await client.Filter(new ActionRequest()
{
    Event = "$registration",
    Status = "$attempted",
    UserId = "user-123",
    RequestToken = "token-from-castle-js",
    Context = Castle.Context.FromHttpRequest(Request)
});
```

### Log

```csharp
await client.Log(new ActionRequest()
{
    Event = "$profile_update",
    UserId = "user-123",
    Context = Castle.Context.FromHttpRequest(Request)
});
```

### Advanced: Build and Send

Each action supports a two-step pattern where you build the JSON request, optionally modify it, and then send it separately.

```csharp
var jsonRequest = client.BuildRiskRequest(new ActionRequest()
{
    Event = "$login",
    UserId = "user-123",
    Context = Castle.Context.FromHttpRequest(Request)
});

// Inspect or modify jsonRequest (JObject) if needed

var response = await client.SendRiskRequest(jsonRequest);
```

This pattern is available for all actions: `BuildRiskRequest` / `SendRiskRequest`, `BuildFilterRequest` / `SendFilterRequest`, `BuildLogRequest` / `SendLogRequest`.

## Lists

Manage lists and their items.

```csharp
var list = await client.CreateList(new CreateListRequest()
{
    Name = "Blocklist",
    Color = "$red",
    PrimaryField = "user.email"
});

var all = await client.GetAllLists();
var fetched = await client.GetList(list.Id);
await client.UpdateList(list.Id, new UpdateListRequest() { Name = "Renamed" });
await client.DeleteList(list.Id);

var matches = await client.QueryLists(new SearchQuery()
{
    Filters = new List<QueryFilter>()
    {
        new QueryFilter() { Field = "name", Op = "$eq", Value = "Blocklist" }
    }
});
```

List items:

```csharp
var item = await client.CreateListItem(list.Id, new CreateListItemRequest()
{
    PrimaryValue = "user@example.com",
    Author = new ListItemAuthor() { Type = "$user", Identifier = "user:123" }
});

await client.GetListItem(list.Id, item.Id);
await client.UpdateListItem(list.Id, item.Id, new UpdateListItemRequest() { Comment = "Flagged" });
await client.QueryListItems(list.Id, new SearchQuery());
await client.CountListItems(list.Id, new CountListItemsRequest());
await client.ArchiveListItem(list.Id, item.Id);
await client.UnarchiveListItem(list.Id, item.Id);
await client.CreateBatchListItems(list.Id, new BatchListItemsRequest()
{
    Items = new List<CreateListItemRequest>() { /* ... */ }
});
```

## Privacy

Request or delete the data Castle stores for a user.

```csharp
await client.RequestUserData(new PrivacyRequest()
{
    Identifier = "user@example.com",
    IdentifierType = "$email"
});

await client.DeleteUserData(new PrivacyRequest()
{
    Identifier = "user@example.com",
    IdentifierType = "$email"
});
```

## Events (enterprise)

Query event data.

```csharp
var schema = await client.EventsSchema();

var events = await client.QueryEvents(new EventsQueryRequest()
{
    Filters = new List<QueryFilter>()
    {
        new QueryFilter() { Field = "name", Op = "$eq", Value = "$login" }
    }
});

var grouped = await client.GroupEvents(new EventsGroupRequest()
{
    Filters = new List<QueryFilter>(),
    GroupBy = new EventsGroupBy() { Fields = new List<string>() { "name" } }
});
```

## Webhooks

Verify the authenticity of incoming Castle webhooks against the `X-Castle-Signature` header.

```csharp
try
{
    Castle.Webhook.Verify(requestBody, Request.Headers["X-Castle-Signature"]);
    // handle the webhook payload
}
catch (CastleWebhookVerificationException)
{
    // reject the request
}
```

By default the API secret is read from the active `CastleConfiguration`; an explicit secret can be passed as a third argument.

## Request Context

Use `Castle.Context.FromHttpRequest()` to extract client context (IP and headers) from the current HTTP request.

### ASP&#46;NET Core

```csharp
public class IndexModel : PageModel
{
    public void OnGet()
    {
        var actionRequest = new ActionRequest()
        {
            Context = Castle.Context.FromHttpRequest(Request),
            Event = "$login",
            UserId = "user-123"
        };
    }
}
```

## Logging

The SDK allows customized logging by way of implementing the `ICastleLogger` interface and passing in an instance as part of the `CastleConfiguration`. Exactly what gets logged can be controlled by setting the `LogLevel` property of `CastleConfiguration`.

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

## Demo application

There is a sample application using ASP&#46;NET Core Razor Pages and this SDK [here](https://github.com/castle/dotnet-example).
