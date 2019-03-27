# Change Log

## master

## 1.2.0 (2019-03-27)
**Features:**
- [#25](https://github.com/castle/castle-dotnet/pull/25) Added client method for calling `archive devices` endpoint.

**Enhancements:**
- [#26](https://github.com/castle/castle-dotnet/pull/26) Guard against missing client method arguments and API secret 
- [#27](https://github.com/castle/castle-dotnet/pull/27) Return null for requests that throw exceptions.

**Bug fixes:**
- [#28](https://github.com/castle/castle-dotnet/pull/28) Include request/response content in INFO-level logging.

## 1.1.1 (2019-03-26)

**Bug fixes:**
- [#23](https://github.com/castle/castle-dotnet/pull/23) Include xml docs in all build configurations.

## 1.1.0 (2019-03-20)

**Features:**
- [#11](https://github.com/castle/castle-dotnet/pull/11) Allow optional `clientId` parameter in the call to GetDevices(), which will set each device's `IsCurrentDevice` property accordingly.
- [#16](https://github.com/castle/castle-dotnet/pull/16) Can get IP address from request headers when using `Castle.Context.FromHttpRequest()`, by providing the helper with a list of header names to look for.